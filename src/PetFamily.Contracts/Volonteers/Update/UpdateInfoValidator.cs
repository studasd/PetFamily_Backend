using FluentValidation;
using PetFamily.Contracts.Extensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Contracts.Volonteers.Update;

public class UpdateInfoValidator : AbstractValidator<UpdateInfoRequest>
{
	public UpdateInfoValidator() 
	{
		RuleFor(r => r.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired("Volunteer Id is not empty"));
	}
}