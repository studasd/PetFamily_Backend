using FluentValidation;
using PetFamily.Contracts.Extensions;

namespace PetFamily.Contracts.Volonteers.Updates.BankingDetails;

public class UpdateBankingDetailsValidatorDTO : AbstractValidator<UpdateBankingDetailsRequestDTO>
{
	public UpdateBankingDetailsValidatorDTO()
	{
		RuleForEach(c => c.BankingDetails)
			.MustBeValueObject(x => Domain.Shared.ValueObjects.BankingDetails.Create(x.Name, x.Description));
	}
}
