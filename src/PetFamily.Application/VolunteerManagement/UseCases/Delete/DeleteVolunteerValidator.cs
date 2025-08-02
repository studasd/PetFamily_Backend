using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Core.Errores;

namespace PetFamily.Application.VolunteerManagement.UseCases.Delete;

public class DeleteVolunteerValidator : AbstractValidator<DeleteVolunteerCommand>
{
	public DeleteVolunteerValidator() 
	{
		RuleFor(r => r.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired("Volunteer Id is not empty"));
	}
}
