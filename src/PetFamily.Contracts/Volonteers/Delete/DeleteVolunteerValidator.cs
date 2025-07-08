using FluentValidation;
using PetFamily.Contracts.Extensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Contracts.Volonteers.Delete;

public class DeleteVolunteerValidator : AbstractValidator<DeleteVolunteerRequest>
{
	public DeleteVolunteerValidator() 
	{
		RuleFor(r => r.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired("Volunteer Id is not empty"));
	}
}
