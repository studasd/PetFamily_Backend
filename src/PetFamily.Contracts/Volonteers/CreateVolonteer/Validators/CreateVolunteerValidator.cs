using FluentValidation;
using FluentValidation.Validators;
using PetFamily.Contracts.Extensions;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerEntities;
using System.Text.RegularExpressions;

namespace PetFamily.Contracts.Volonteers.CreateVolonteer.Validators;

public class CreateVolunteerValidator : AbstractValidator<CreateVolunteerRequest>
{
	public CreateVolunteerValidator()
	{
		RuleFor(c => new { c.Firstname, c.Lastname, c.Surname })
			.MustBeValueObject(x => VolunteerName.Create(x.Firstname, x.Lastname, x.Surname));


		RuleFor(c => c.Email)
			.NotEmpty().WithMessage(Error.NotFoundSerialize("email_invalid", "Email is not empty"))
			.EmailAddress().WithMessage(Error.ValidationSerialize("email_invalid", "Email not valid"));

		RuleFor(c => c.Description)
			.NotEmpty().WithMessage(Error.NotFoundSerialize("description_invalid", "Description is not empty"))
			.MaximumLength(5).WithMessage(Error.ValidationSerialize("description_invalid", "Description maximum lenght: 100"));

		RuleFor(c => c.ExperienceYears)
			.InclusiveBetween(0, 100).WithMessage(Error.ValidationSerialize("experience_years_invalid", "Experience years not valid"));


		RuleFor(c => c.Phone)
			.MustBeValueObject(Phone.Create);


		RuleFor(c => c.BankingDetails).SetValidator(new BankingDetailsDTOValidator());

		RuleForEach(c => c.SocialNetworks).SetValidator(new SocialNetworkDTOValidator());
	}
}
