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
			.NotEmpty().WithError(Errors.General.ValueIsRequired("Email is not empty"))
			.EmailAddress().WithError(Errors.General.ValueIsInvalid("Email not valid"));

		RuleFor(c => c.Description)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("Description is not empty"))
			.MaximumLength(100).WithError(Errors.General.ValueIsInvalid("Description maximum lenght: 100"));

		RuleFor(c => c.ExperienceYears)
			.InclusiveBetween(0, 100).WithError(Errors.General.ValueIsInvalid("Experience years not valid"));


		RuleFor(c => c.Phone)
			.MustBeValueObject(Phone.Create);


		RuleFor(c => c.BankingDetails).SetValidator(new BankingDetailsDTOValidator());

		RuleForEach(c => c.SocialNetworks).SetValidator(new SocialNetworkDTOValidator());
	}
}
