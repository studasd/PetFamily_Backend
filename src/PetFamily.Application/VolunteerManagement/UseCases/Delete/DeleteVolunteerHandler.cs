using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Contracts.RequestVolonteers;
using PetFamily.Domain.Shared.Errores;

namespace PetFamily.Application.VolunteerManagement.UseCases.Delete;

public class DeleteVolunteerHandler
{
	private readonly IVolunteerRepository volunteerRepository;
	private readonly IValidator<DeleteVolunteerCommand> validator;
	private readonly ILogger<DeleteVolunteerHandler> logger;

	public DeleteVolunteerHandler(
		IVolunteerRepository volunteerRepository, 
		IValidator<DeleteVolunteerCommand> validator,
		ILogger<DeleteVolunteerHandler> logger)
	{
		this.volunteerRepository = volunteerRepository;
		this.validator = validator;
		this.logger = logger;
	}

	public async Task<Result<Guid, ErrorList>> HandleAsync(DeleteVolunteerCommand command, CancellationToken token)
	{
		var validateResult = await validator.ValidateAsync(command, token);
		if (validateResult.IsValid == false)
			return validateResult.ToErrorList();

		var volunteerResult = await volunteerRepository.GetByIdAsync(command.VolunteerId, token);

		if (volunteerResult.IsFailure)
			return volunteerResult.Error.ToErrorList();

		if (command.IsSoftDelete == true)
		{
			volunteerResult.Value.Delete();

			await volunteerRepository.SaveAsync(token);
		}
		else
		{
			await volunteerRepository.DeleteAsync(volunteerResult.Value, token);
		}


		logger.LogInformation("Deleted volunteer with id {volunteerId}", volunteerResult.Value.Id);

		return volunteerResult.Value.Id.Value;
	}
}
