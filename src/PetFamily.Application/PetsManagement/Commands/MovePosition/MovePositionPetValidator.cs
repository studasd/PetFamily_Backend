using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared.Errores;

namespace PetFamily.Application.PetsManagement.Commands.MovePosition;

public class MovePositionPetValidator : AbstractValidator<MovePositionPetCommand>
{
	public MovePositionPetValidator()
	{
		RuleFor(c => c.VolunteerId)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("VolunteerId is not empty"));

		RuleFor(c => c.PetId)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("PetId is not empty"));

		RuleFor(c => c.NewPosition)
			.InclusiveBetween(1, 100).WithError(Errors.General.ValueIsInvalid("New position not valid. Range 1-100"));
	}
}