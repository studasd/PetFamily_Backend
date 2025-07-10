using FluentValidation;
using PetFamily.Contracts.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Contracts.Volonteers.Update;

public class UpdateInfoDTOValidator : AbstractValidator<UpdateInfoRequestDTO>
{
	public UpdateInfoDTOValidator() 
	{
		RuleFor(c => c.Name)
			.MustBeValueObject(x => VolunteerName.Create(x.Firstname, x.Lastname, x.Surname));

		RuleFor(c => c.Email)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("Email is not empty"))
			.EmailAddress().WithError(Errors.General.ValueIsInvalid("Email not valid"));

		RuleFor(c => c.Description)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("Description is not empty"))
			.MaximumLength(100).WithError(Errors.General.ValueIsInvalid("Description maximum lenght: 100"));
	}
}