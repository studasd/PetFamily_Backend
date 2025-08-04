using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.FileProvider;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Application.VolunteerManagement;

namespace PetFamily.Volunteers.Application.PetsManagement.Commands.DeletePhotos;

public class DeletePhotosPetHandler : ICommandHandler<Guid, DeletePhotosPetCommand>
{
	private readonly IFileProvider fileProvider;
	private readonly IVolunteerRepository volunteerRepository;
	private readonly IValidator<DeletePhotosPetCommand> validator;
	private readonly ILogger<DeletePhotosPetHandler> logger;
	private const string BUCKET_NAME = "photos";

	public DeletePhotosPetHandler(
		IFileProvider fileProvider,
		IVolunteerRepository volunteerRepository,
		IValidator<DeletePhotosPetCommand> validator,
		ILogger<DeletePhotosPetHandler> logger)
	{
		this.fileProvider = fileProvider;
		this.volunteerRepository = volunteerRepository;
		this.validator = validator;
		this.logger = logger;
	}

	public async Task<Result<Guid, ErrorList>> HandleAsync(DeletePhotosPetCommand command, CancellationToken token)
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

		foreach (var file in command.DeleteFiles)
		{
			var deleteResult = await fileProvider.DeleteFileAsync(new FileInform(file.ToString(), BUCKET_NAME), token);
			if (deleteResult.IsFailure)
				return deleteResult.Error.ToErrorList();
		}

		var deleteFiles = command.DeleteFiles.Select(f => FileStorage.Create(f.ToString()).Value);

		petResult.Value.DeletePhotos(deleteFiles);

		await volunteerRepository.SaveAsync(token);

		logger.LogInformation("Delete pet {id} photos.", command.PetId);

		return petResult.Value.Id.Value;
	}
}
