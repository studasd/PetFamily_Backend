using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.PetsManagement.Commands.UpdateStatus;

public class UpdatePetStatusValidator : AbstractValidator<UpdatePetStatusCommand>
{
	public UpdatePetStatusValidator()
	{
		RuleFor(c => c.VolunteerId)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("VolunteerId is not empty"));

		RuleFor(c => c.PetId)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("PetId is not empty"));

		RuleFor(c => c.HelpStatus)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("HelpStatus is not empty"));
	}
}
