using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Core.Errores;
using PetFamily.Core.ValueObjects;

namespace PetFamily.Application.VolunteerManagement.UseCases.Updates.BankingDetails;

public class UpdateBankingDetailsCommandValidator : AbstractValidator<UpdateBankingDetailsCommand>
{
	public UpdateBankingDetailsCommandValidator()
	{
		RuleFor(r => r.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired("Volunteer Id is not empty"));

		RuleForEach(c => c.BankingDetails)
			.MustBeValueObject(x => Core.ValueObjects.BankingDetails.Create(x.Name, x.Description));
	}
}
