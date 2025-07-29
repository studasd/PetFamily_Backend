using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Messaging;
using PetFamily.Application.PetsManagement.Commands.UploadPhotos;
using PetFamily.Application.VolunteerManagement;
using PetFamily.Domain.Shared.Errores;

namespace PetFamily.Application.PetsManagement.Commands.Delete;

public class DeletePetHandler : ICommandHandler<Guid, DeletePetCommand>
{
	private readonly IVolunteerRepository volunteerRepository;
	private readonly IValidator<DeletePetCommand> validator;
	private readonly IMessageQueue<IEnumerable<FileInform>> messageQueue;
	private readonly ILogger<DeletePetHandler> logger;

	public DeletePetHandler(
		IVolunteerRepository volunteerRepository,
		IValidator<DeletePetCommand> validator,
		IMessageQueue<IEnumerable<FileInform>> messageQueue,
		ILogger<DeletePetHandler> logger)
	{
		this.volunteerRepository = volunteerRepository;
		this.validator = validator;
		this.messageQueue = messageQueue;
		this.logger = logger;
	}

	public async Task<Result<Guid, ErrorList>> HandleAsync(DeletePetCommand command, CancellationToken token)
	{
		var validateResult = await validator.ValidateAsync(command, token);
		if (validateResult.IsValid == false)
			return validateResult.ToErrorList();

		var volunteerResult = await volunteerRepository.GetByIdAsync(command.VolunteerId, token);

		if (volunteerResult.IsFailure)
			return volunteerResult.Error.ToErrorList();

		if (command.IsSoftDelete == true)
		{
			var petResult = volunteerResult.Value.DeletePet(command.PetId);
			if (petResult.IsFailure)
				return petResult.Error.ToErrorList();

			await volunteerRepository.SaveAsync(token);
		}
		else
		{
			var petResult = volunteerResult.Value.DeletePetHard(command.PetId);
			if (petResult.IsFailure)
				return petResult.Error.ToErrorList();

			var filesDelete = petResult.Value.FileStorages
				.Select(f => new FileInform(f.PathToStorage, UploadPhotosPetHandler.BUCKET_NAME));

			await messageQueue.WriteAsync(filesDelete, token);

			await volunteerRepository.SaveAsync(token);
		}


		logger.LogInformation("Deleted pet ID {petId} with volunteer id {volunteerId}", command.PetId, volunteerResult.Value.Id);

		return command.PetId;
	}
}
