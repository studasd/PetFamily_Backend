using FluentValidation;
using PetFamily.Contracts.Extensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Contracts.Volonteers.Updates.BankingDetails;

public class UpdateBankingDetailsValidator : AbstractValidator<UpdateBankingDetailsRequest>
{
	public UpdateBankingDetailsValidator()
	{
		RuleFor(r => r.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired("Volunteer Id is not empty"));
	}
}
