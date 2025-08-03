using FluentValidation;
using PetFamily.SharedKernel;
using PetFamily.Core.Extensions;

namespace PetFamily.Volunteers.Application.SpeciesManagemets.Commands.Delete;

public class DeleteSpeciesCommandValidator : AbstractValidator<DeleteSpeciesCommand>
{
	public DeleteSpeciesCommandValidator()
	{
		RuleFor(r => r.SpeciesId).NotEmpty().WithError(Errors.General.ValueIsRequired("Species Id is not empty"));
	}
}
