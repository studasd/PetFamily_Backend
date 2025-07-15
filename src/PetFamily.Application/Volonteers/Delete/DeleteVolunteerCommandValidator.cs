using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Contracts.RequestVolonteers;
using PetFamily.Domain.Shared.Errores;

namespace PetFamily.Application.Volonteers.Delete;

public class DeleteVolunteerCommandValidator : AbstractValidator<DeleteVolunteerCommand>
{
	public DeleteVolunteerCommandValidator() 
	{
		RuleFor(r => r.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired("Volunteer Id is not empty"));
	}
}
