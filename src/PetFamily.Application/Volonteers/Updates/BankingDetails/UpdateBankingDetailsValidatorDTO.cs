using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Contracts.Volonteers;

namespace PetFamily.Application.Volonteers.Updates.BankingDetails;

public class UpdateBankingDetailsValidatorDTO : AbstractValidator<UpdateBankingDetailsRequestDTO>
{
	public UpdateBankingDetailsValidatorDTO()
	{
		RuleForEach(c => c.BankingDetails)
			.MustBeValueObject(x => Domain.Shared.ValueObjects.BankingDetails.Create(x.Name, x.Description));
	}
}
