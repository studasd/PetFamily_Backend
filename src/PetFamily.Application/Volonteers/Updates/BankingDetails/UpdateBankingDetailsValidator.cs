using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Contracts.Volonteers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.Updates.BankingDetails;

public class UpdateBankingDetailsValidator : AbstractValidator<UpdateBankingDetailsRequest>
{
	public UpdateBankingDetailsValidator()
	{
		RuleFor(r => r.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired("Volunteer Id is not empty"));
	}
}
