using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared.Errores;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volonteers.Updates.BankingDetails;

public class UpdateBankingDetailsCommandValidator : AbstractValidator<UpdateBankingDetailsCommand>
{
	public UpdateBankingDetailsCommandValidator()
	{
		RuleFor(r => r.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired("Volunteer Id is not empty"));

		RuleForEach(c => c.BankingDetails)
			.MustBeValueObject(x => PetFamily.Domain.Shared.ValueObjects.BankingDetails.Create(x.Name, x.Description));
	}
}
