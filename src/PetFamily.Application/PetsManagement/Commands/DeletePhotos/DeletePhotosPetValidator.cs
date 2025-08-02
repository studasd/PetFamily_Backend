using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Core.Errores;

namespace PetFamily.Application.PetsManagement.Commands.DeletePhotos;

public class DeletePhotosPetValidator : AbstractValidator<DeletePhotosPetCommand>
{
	public DeletePhotosPetValidator()
	{
		RuleFor(c => c.VolunteerId)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("VolunteerId is not empty"));

		RuleFor(c => c.PetId)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("PetId is not empty"));

		RuleForEach(c => c.DeleteFiles)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("Filename is not empty"));
	}
}
