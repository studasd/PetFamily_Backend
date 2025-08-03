using FluentValidation;
using PetFamily.SharedKernel;
using PetFamily.Core.Extensions;

namespace PetFamily.Specieses.Application.Commands.Delete;

public class DeleteSpeciesCommandValidator : AbstractValidator<DeleteSpeciesCommand>
{
	public DeleteSpeciesCommandValidator()
	{
		RuleFor(r => r.SpeciesId).NotEmpty().WithError(Errors.General.ValueIsRequired("Species Id is not empty"));
	}
}
