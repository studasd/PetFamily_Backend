using FluentValidation;

namespace PetFamily.Contracts.Volonteers.CreateVolonteer.Validators;

public class BankingDetailsDTOValidator : AbstractValidator<BankingDetailsDTO>
{
	public BankingDetailsDTOValidator()
	{
		RuleFor(c => c.Name)
			.NotEmpty().WithMessage("Banking name is not empty")
			.MaximumLength(50).WithErrorCode("banking_name_invalid").WithMessage("Banking name maximum lenght: 50");

		RuleFor(c => c.Description)
			.NotEmpty().WithMessage("Banking description is not empty")
			.MaximumLength(200).WithErrorCode("banking_description_invalid").WithMessage("Banking description maximum lenght: 200");
	}
}
