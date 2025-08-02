using FluentValidation;
using PetFamily.Contracts.DTOs;
using PetFamily.Core.Errores;
using PetFamily.Core.Extensions;

namespace PetFamily.Core.DTOs.Validators;

public class UploadFileDtoValidator : AbstractValidator<UploadFileDto>
{
	public UploadFileDtoValidator()
	{
		RuleFor(c => c.FileName)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("FileName is not empty"));

		RuleFor(c => c.Content)
			.Must(c => c.Length < 5000000).WithError(Errors.General.ValueIsInvalid("Photo max 5Mb"));
	}
}
