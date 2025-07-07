using FluentValidation;
using FluentValidation.Validators;
using System.Text.RegularExpressions;

namespace PetFamily.Contracts.Volonteers.CreateVolonteer.Validators;

public class CreateVolunteerValidator : AbstractValidator<CreateVolunteerRequest>
{
	public CreateVolunteerValidator()
	{
		RuleFor(c => c.Firstname)
			.NotEmpty().WithErrorCode("firstname_invalid").WithMessage("Firstname is not empty")
			.MaximumLength(20).WithErrorCode("firstname_invalid").WithMessage("Firstname maximum lenght: 20");

		RuleFor(c => c.Lastname)
			.NotEmpty().WithErrorCode("lastname_invalid").WithMessage("Lastname is not empty")
			.MaximumLength(20).WithErrorCode("lastname_invalid").WithMessage("Lastname maximum lenght: 20");

		RuleFor(c => c.Surname)
			.NotEmpty().WithErrorCode("surname_invalid").WithMessage("Surname is not empty")
			.MaximumLength(20).WithErrorCode("surname_invalid").WithMessage("Surname maximum lenght: 20");

		RuleFor(c => c.Email)
			.NotEmpty().WithErrorCode("email_invalid").WithMessage("Email is not empty")
			.EmailAddress().WithErrorCode("email_invalid").WithMessage("Email not valid");

		RuleFor(c => c.Description)
			.NotEmpty().WithErrorCode("description_invalid").WithMessage("Description is not empty")
			.MaximumLength(100).WithErrorCode("description_invalid").WithMessage("Description maximum lenght: 100");

		RuleFor(c => c.ExperienceYears)
			.InclusiveBetween(0, 100).WithErrorCode("phone_invalid").WithMessage("PhoneNumber not valid");

		RuleFor(c => c.Phone)
			.NotEmpty().WithErrorCode("phone_invalid").WithMessage("Phone is not empty")
			.MaximumLength(15).WithErrorCode("phone_invalid").WithMessage("Phone is not empty")
			.Matches(new Regex(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}")).WithMessage("Phone not valid");

		RuleFor(c => c.BankingDetails).Null().SetValidator(new BankingDetailsDTOValidator());
		RuleForEach(c => c.SocialNetworks).Null().SetValidator(new SocialNetworkDTOValidator());

	}
}
