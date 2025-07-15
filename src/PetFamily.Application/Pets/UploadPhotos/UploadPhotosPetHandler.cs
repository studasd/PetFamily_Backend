using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Messaging;
using PetFamily.Application.Providers;
using PetFamily.Application.Volonteers;
using PetFamily.Domain.Shared.Errores;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Pets.UploadPhotos;

public class UploadPhotosPetHandler // CreatePetService
{
	private readonly IFileProvider fileProvider;
	private readonly IVolunteerRepository volunteerRepository;
	private readonly IUnitOfWork unitOfWork;
	private readonly IMessageQueue<IEnumerable<FileInform>> messageQueue;
	private readonly ILogger<UploadPhotosPetHandler> logger;
	private const string BUCKET_NAME = "photos";

	public UploadPhotosPetHandler(
		IFileProvider fileProvider,
		IVolunteerRepository volunteerRepository,
		IUnitOfWork unitOfWork,
		IMessageQueue<IEnumerable<FileInform>> messageQueue,
		ILogger<UploadPhotosPetHandler> logger)
	{
		this.fileProvider = fileProvider;
		this.volunteerRepository = volunteerRepository;
		this.unitOfWork = unitOfWork;
		this.messageQueue = messageQueue;
		this.logger = logger;
	}

	public async Task<Result<Guid, ErrorList>> HandleAsync(UploadPhotosPetCommand command, CancellationToken token)
	{
		var volunteerResult = await volunteerRepository.GetByIdAsync(command.VolunteerId, token);
		if (volunteerResult.IsFailure)
			return volunteerResult.Error.ToErrorList();

		var petResult = volunteerResult.Value.Pets.FirstOrDefault(p => p.Id.Value == command.PetId);
		if (petResult == null)
			return Errors.General.NotFound(command.PetId).ToErrorList();


		var transaction = await unitOfWork.BeginTransactionAsync(token);

		try
		{
			List<FileStorage> filePaths = [];
			List<FileData> fileContents = [];
			foreach (var file in command.UploadFiles)
			{
				var extension = Path.GetExtension(file.FileName);

				var filePath = FileStorage.Create(Guid.NewGuid(), extension);
				if (filePath.IsFailure)
					return filePath.Error.ToErrorList();

				var fileContent = new FileData(file.Content, new FileInform(filePath.Value.PathToStorage, BUCKET_NAME));
				fileContents.Add(fileContent);
				filePaths.Add(filePath.Value);
			}

			var uploadResult = await fileProvider.UploadFilesAsync(fileContents, token);
			if (uploadResult.IsFailure)
			{
				await messageQueue.WriteAsync(fileContents.Select(f => f.FileInform), token);
				
				return uploadResult.Error.ToErrorList();
			}

			petResult.AddPhotos(filePaths);

			await unitOfWork.SaveChangesAsync(token);

			transaction.Commit();

			logger.LogInformation("Upload pet {id} upload photos.", command.PetId);

			return petResult.Id.Value; //Guid.NewGuid();
		}
		catch(Exception e)
		{
			logger.LogError(e, "Can not upload pet photos - {id} in transaction", command.PetId);

			transaction.Rollback();

			return Error.Failure("volunteer_peet_failure", "Can not add pet photos").ToErrorList();
		}
	}
}
