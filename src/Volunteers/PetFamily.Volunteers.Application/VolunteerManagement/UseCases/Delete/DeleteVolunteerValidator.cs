using FluentValidation;
using PetFamily.Core.Errores;
using PetFamily.Core.Extensions;

namespace PetFamily.Volunteers.Application.VolunteerManagement.UseCases.Delete;

public class DeleteVolunteerValidator : AbstractValidator<DeleteVolunteerCommand>
{
	public DeleteVolunteerValidator() 
	{
		RuleFor(r => r.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired("Volunteer Id is not empty"));
	}
}
