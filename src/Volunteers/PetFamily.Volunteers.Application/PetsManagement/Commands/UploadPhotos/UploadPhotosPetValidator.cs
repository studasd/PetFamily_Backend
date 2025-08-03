using FluentValidation;
using PetFamily.Core.DTOs.Validators;
using PetFamily.SharedKernel;
using PetFamily.Core.Extensions;

namespace PetFamily.Volunteers.Application.PetsManagement.Commands.UploadPhotos;

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
