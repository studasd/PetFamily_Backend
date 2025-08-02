using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Core.Errores;

namespace PetFamily.Application.PetsManagement.Commands.UpdatePrimePhoto;

public class UpdatePetPrimePhotoValidator : AbstractValidator<UpdatePetPrimePhotoCommand>
{
	public UpdatePetPrimePhotoValidator()
	{
		RuleFor(c => c.VolunteerId)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("VolunteerId is not empty"));

		RuleFor(c => c.PetId)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("PetId is not empty"));

		RuleFor(c => c.PathPhoto)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("PathPhoto is not empty"));
	}
}