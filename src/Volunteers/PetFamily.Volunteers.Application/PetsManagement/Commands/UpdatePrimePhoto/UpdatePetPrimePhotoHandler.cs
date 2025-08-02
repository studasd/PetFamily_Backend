using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Errores;
using PetFamily.Core.Extensions;
using PetFamily.Volunteers.Application.VolunteerManagement;

namespace PetFamily.Volunteers.Application.PetsManagement.Commands.UpdatePrimePhoto;

public class UpdatePetPrimePhotoHandler : ICommandHandler<Guid, UpdatePetPrimePhotoCommand>
{
	private readonly IVolunteerRepository volunteerRepository;
	private readonly IValidator<UpdatePetPrimePhotoCommand> validator;
	private readonly ILogger<UpdatePetPrimePhotoHandler> logger;

	public UpdatePetPrimePhotoHandler(
		IVolunteerRepository volunteerRepository,
		IValidator<UpdatePetPrimePhotoCommand> validator,
		ILogger<UpdatePetPrimePhotoHandler> logger)
	{
		this.volunteerRepository = volunteerRepository;
		this.validator = validator;
		this.logger = logger;
	}

	public async Task<Result<Guid, ErrorList>> HandleAsync(UpdatePetPrimePhotoCommand command, CancellationToken token)
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

		var updateResult = volunteerResult.Value.UpdatePetPrimePhoto(command.PetId, command.PathPhoto);
		if (updateResult.IsFailure)
			return updateResult.Error.ToErrorList();

		await volunteerRepository.SaveAsync(token);

		logger.LogInformation(
			"Pet updated prime photo with ID {petId} updated by volunteer with ID {volunteerId}",
			command.PetId,
			command.VolunteerId
		);

		await volunteerRepository.SaveAsync(token);

		return command.PetId;
	}
}
