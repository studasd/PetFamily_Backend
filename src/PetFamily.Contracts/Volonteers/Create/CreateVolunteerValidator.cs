using FluentValidation;
using PetFamily.Contracts.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Contracts.Volonteers.Create;

public class CreateVolunteerValidator : AbstractValidator<CreateVolunteerRequest>
{
	public CreateVolunteerValidator()
	{
		RuleFor(c => c.Name)
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


		RuleForEach(c => c.BankingDetails)
			.MustBeValueObject(x => BankingDetails.Create(x.Name, x.Description));

		RuleForEach(c => c.SocialNetworks)
			.MustBeValueObject(x => SocialNetwork.Create(x.Name, x.Link));

	}
}
