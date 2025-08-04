using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.VolunteerManagement.UseCases.Delete;

public class DeleteVolunteerValidator : AbstractValidator<DeleteVolunteerCommand>
{
	public DeleteVolunteerValidator() 
	{
		RuleFor(r => r.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired("Volunteer Id is not empty"));
	}
}
