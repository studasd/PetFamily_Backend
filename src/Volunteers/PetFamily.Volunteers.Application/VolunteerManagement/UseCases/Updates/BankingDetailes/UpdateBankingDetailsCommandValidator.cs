using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Volunteers.Application.VolunteerManagement.UseCases.Updates.BankingDetailes;

public class UpdateBankingDetailsCommandValidator : AbstractValidator<UpdateBankingDetailsCommand>
{
	public UpdateBankingDetailsCommandValidator()
	{
		RuleFor(r => r.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired("Volunteer Id is not empty"));

		RuleForEach(c => c.BankingDetails)
			.MustBeValueObject(x => BankingDetails.Create(x.Name, x.Description));
	}
}
