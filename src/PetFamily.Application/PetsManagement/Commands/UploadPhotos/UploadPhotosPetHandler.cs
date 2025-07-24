using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Messaging;
using PetFamily.Application.VolunteerManagement;
using PetFamily.Domain.Shared.Errores;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.PetsManagement.Commands.UploadPhotos;

public class UploadPhotosPetHandler : ICommandHandler<Guid, UploadPhotosPetCommand> // CreatePetService
{
	private readonly IFileProvider fileProvider;
	private readonly IVolunteerRepository volunteerRepository;
	private readonly IUnitOfWork unitOfWork;
	private readonly IValidator<UploadPhotosPetCommand> validator;
	private readonly IMessageQueue<IEnumerable<FileInform>> messageQueue;
	private readonly ILogger<UploadPhotosPetHandler> logger;
	public const string BUCKET_NAME = "photos";

	public UploadPhotosPetHandler(
		IFileProvider fileProvider,
		IVolunteerRepository volunteerRepository,
		IUnitOfWork unitOfWork,
		IValidator<UploadPhotosPetCommand> validator,
		IMessageQueue<IEnumerable<FileInform>> messageQueue,
		ILogger<UploadPhotosPetHandler> logger)
	{
		this.fileProvider = fileProvider;
		this.volunteerRepository = volunteerRepository;
		this.unitOfWork = unitOfWork;
		this.validator = validator;
		this.messageQueue = messageQueue;
		this.logger = logger;
	}

	public async Task<Result<Guid, ErrorList>> HandleAsync(UploadPhotosPetCommand command, CancellationToken token)
	{
		var validateResult = await validator.ValidateAsync(command, token);
		if (validateResult.IsValid == false)
			return validateResult.ToErrorList();

		var volunteerResult = await volunteerRepository.GetByIdAsync(command.VolunteerId, token);
		if (volunteerResult.IsFailure)
			return volunteerResult.Error.ToErrorList();

		var petResult = volunteerResult.Value.GetPetById(command.PetId);
		if (petResult.IsFailure)
			return petResult.Error.ToErrorList();


		var transaction = await unitOfWork.BeginTransactionAsync(token);

		try
		{
			List<FileStorage> filePaths = [];
			List<FileData> fileDatas = [];
			foreach (var file in command.UploadFiles)
			{
				var extension = Path.GetExtension(file.FileName);

				var filePath = FileStorage.Create(Guid.NewGuid(), extension);
				if (filePath.IsFailure)
					return filePath.Error.ToErrorList();

				var fileData = new FileData(file.Content, new FileInform(filePath.Value.PathToStorage, BUCKET_NAME));
				
				fileDatas.Add(fileData);
				filePaths.Add(filePath.Value);
			}

			var uploadResult = await fileProvider.UploadFilesAsync(fileDatas, token);
			if (uploadResult.IsFailure)
			{
				await messageQueue.WriteAsync(fileDatas.Select(f => f.FileInform), token);
				
				return uploadResult.Error.ToErrorList();
			}

			petResult.Value.AddPhotos(filePaths);

			await unitOfWork.SaveChangesAsync(token);

			transaction.Commit();

			logger.LogInformation("Upload pet {id} upload photos.", command.PetId);

			return petResult.Value.Id.Value; //Guid.NewGuid();
		}
		catch(Exception e)
		{
			logger.LogError(e, "Can not upload pet photos - {id} in transaction", command.PetId);

			transaction.Rollback();

			return Error.Failure("volunteer_peet_failure", "Can not add pet photos").ToErrorList();
		}
	}
}
