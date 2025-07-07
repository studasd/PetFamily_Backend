using FluentValidation;
using PetFamily.Contracts.Extensions;
using PetFamily.Domain.Entities;

namespace PetFamily.Contracts.Volonteers.Create.Validators;

public class BankingDetailsDTOValidator : AbstractValidator<BankingDetailsDTO>
{
	public BankingDetailsDTOValidator()
	{
		RuleFor(c => new { c.Name, c.Description })
			.MustBeValueObject(x => BankingDetails.Create(x.Name, x.Description));

		//RuleFor(c => c.Name)
		//	.NotEmpty().WithMessage("Banking name is not empty")
		//	.MaximumLength(50).WithErrorCode("banking_name_invalid").WithMessage("Banking name maximum lenght: 50");

		//RuleFor(c => c.Description)
		//	.NotEmpty().WithMessage("Banking description is not empty")
		//	.MaximumLength(200).WithErrorCode("banking_description_invalid").WithMessage("Banking description maximum lenght: 200");
	}
}
