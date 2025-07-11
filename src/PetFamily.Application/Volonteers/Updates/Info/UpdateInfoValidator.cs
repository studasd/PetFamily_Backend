using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Contracts.Volonteers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.Updates.Info;

public class UpdateInfoValidator : AbstractValidator<UpdateInfoRequest>
{
	public UpdateInfoValidator() 
	{
		RuleFor(r => r.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired("Volunteer Id is not empty"));
	}
}
