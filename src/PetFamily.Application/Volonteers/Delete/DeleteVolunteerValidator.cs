using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Contracts.Volonteers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.Delete;

public class DeleteVolunteerValidator : AbstractValidator<DeleteVolunteerRequest>
{
	public DeleteVolunteerValidator() 
	{
		RuleFor(r => r.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired("Volunteer Id is not empty"));
	}
}
