using FluentValidation;
using PetFamily.Application.DTOValidators;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared.Errores;

namespace PetFamily.Application.Pets.UploadPhotos;

public class UploadPhotosPetValidator : AbstractValidator<UploadPhotosPetCommand>
{
	public UploadPhotosPetValidator()
	{
		RuleFor(c => c.VolunteerId)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("VolunteerId is not empty"));

		RuleFor(c => c.PetId)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("PetId is not empty"));

		RuleForEach(c => c.UploadFiles).SetValidator(new UploadFileDtoValidator());
	}
}
