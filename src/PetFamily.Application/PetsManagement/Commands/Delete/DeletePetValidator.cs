using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared.Errores;

namespace PetFamily.Application.PetsManagement.Commands.Delete;

public class DeletePetValidator : AbstractValidator<DeletePetCommand>
{
	public DeletePetValidator()
	{
		RuleFor(r => r.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired("Volunteer Id is not empty"));
		RuleFor(r => r.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired("Pet Id is not empty"));
	}
}
