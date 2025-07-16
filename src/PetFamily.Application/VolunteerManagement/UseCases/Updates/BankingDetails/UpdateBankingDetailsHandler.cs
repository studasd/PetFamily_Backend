using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Contracts.RequestVolonteers;
using PetFamily.Domain.Shared.Errores;

namespace PetFamily.Application.VolunteerManagement.UseCases.Updates.BankingDetails;

public class UpdateBankingDetailsHandler
{
	private readonly IVolunteerRepository volunteerRepository;
	private readonly IValidator<UpdateBankingDetailsCommand> validator;
	private readonly ILogger<UpdateBankingDetailsHandler> logger;

	public UpdateBankingDetailsHandler(
		IVolunteerRepository volunteerRepository, 
		IValidator<UpdateBankingDetailsCommand> validator,
		ILogger<UpdateBankingDetailsHandler> logger)
	{
		this.volunteerRepository = volunteerRepository;
		this.validator = validator;
		this.logger = logger;
	}

	public async Task<Result<Guid, ErrorList>> HandleAsync(UpdateBankingDetailsCommand command, CancellationToken token)
	{
		var validateResult = await validator.ValidateAsync(command, token);
		if (validateResult.IsValid == false)
			return validateResult.ToErrorList();

		var volunteerResult = await volunteerRepository.GetByIdAsync(command.VolunteerId, token);

		if (volunteerResult.IsFailure)
			return volunteerResult.Error.ToErrorList();

		var bankingDetailsResult = command.BankingDetails.Select(s => Domain.Shared.ValueObjects.BankingDetails.Create(s.Name, s.Description).Value);

		volunteerResult.Value.UpdateBankingDetails(bankingDetailsResult);

		await volunteerRepository.SaveAsync(token);

		logger.LogInformation("Updated volunteer banking details {bankings} with id {volunteerId}", bankingDetailsResult, volunteerResult.Value.Id);

		return volunteerResult.Value.Id.Value;
	}
}