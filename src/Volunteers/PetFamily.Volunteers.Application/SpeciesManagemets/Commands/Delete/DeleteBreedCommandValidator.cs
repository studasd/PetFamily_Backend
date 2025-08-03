using FluentValidation;
using PetFamily.SharedKernel;
using PetFamily.Core.Extensions;

namespace PetFamily.Volunteers.Application.SpeciesManagemets.Commands.Delete;

public class DeleteBreedCommandValidator : AbstractValidator<DeleteBreedCommand>
{
	public DeleteBreedCommandValidator()
	{
		RuleFor(r => r.SpeciesId).NotEmpty().WithError(Errors.General.ValueIsRequired("Species Id is not empty"));

		RuleFor(r => r.BreedId).NotEmpty().WithError(Errors.General.ValueIsRequired("Breed Id is not empty"));
	}
}