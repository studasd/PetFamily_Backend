using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared.Errores;

namespace PetFamily.Application.PetsManagement.Commands.UpdateStatus;

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
