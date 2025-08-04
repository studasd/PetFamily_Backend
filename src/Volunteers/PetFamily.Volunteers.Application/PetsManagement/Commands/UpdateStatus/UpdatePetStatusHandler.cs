using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Volunteers.Contracts.Enums;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Application.VolunteerManagement;
using PetFamily.Volunteers.Domain.IDs;

namespace PetFamily.Volunteers.Application.PetsManagement.Commands.UpdateStatus;

public class UpdatePetStatusHandler : ICommandHandler<Guid, UpdatePetStatusCommand>
{
	private readonly IVolunteerRepository volunteerRepository;
	private readonly IValidator<UpdatePetStatusCommand> validator;
	private readonly ILogger<UpdatePetStatusHandler> logger;

	public UpdatePetStatusHandler(
		IVolunteerRepository volunteerRepository,
		IValidator<UpdatePetStatusCommand> validator,
		ILogger<UpdatePetStatusHandler> logger)
	{
		this.volunteerRepository = volunteerRepository;
		this.validator = validator;
		this.logger = logger;
	}

	public async Task<Result<Guid, ErrorList>> HandleAsync(UpdatePetStatusCommand command, CancellationToken token)
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

		if (command.HelpStatus != PetHelpStatuses.LookingHome && 
			command.HelpStatus != PetHelpStatuses.NeedsHelp)
			return Error.Failure("pet_status_invalid", "Update pet status invalid").ToErrorList();

		var petId = PetId.Create(command.PetId);

		var updateResult = volunteerResult.Value.UpdatePetStatus(petId, command.HelpStatus);
		if (updateResult.IsFailure)
			return updateResult.Error.ToErrorList();

		logger.LogInformation(
				"Pet status with ID {petId} updated by volunteer with ID {volunteerId}",
				command.PetId,
				command.VolunteerId
			);

		await volunteerRepository.SaveAsync(token);

		return command.PetId;
	}
}
