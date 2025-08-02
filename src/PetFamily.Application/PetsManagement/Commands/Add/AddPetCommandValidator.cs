using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Core.Errores;
using PetFamily.Core.ValueObjects;

namespace PetFamily.Application.PetsManagement.Commands.Add;

public class AddPetCommandValidator : AbstractValidator<AddPetCommand>
{
	public AddPetCommandValidator()
	{
		RuleFor(c => c.Name)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("Name is not empty"));

		RuleFor(c => c.Description)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("Description is not empty"))
			.MaximumLength(100).WithError(Errors.General.ValueIsInvalid("Description maximum lenght: 100"));

		RuleFor(c => c.BreedId)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("Breed is not empty"));

		RuleFor(c => c.SpeciesId)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("Species is not empty"));

		RuleFor(c => c.Color)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("Color is not empty"));

		RuleFor(c => c.Weight)
			.InclusiveBetween(0, 100).WithError(Errors.General.ValueIsInvalid("Weight years not valid"));

		RuleFor(c => c.Height)
			.InclusiveBetween(0, 100).WithError(Errors.General.ValueIsInvalid("Height years not valid"));

		RuleFor(c => c.Phone)
			.MustBeValueObject(Phone.Create);

		RuleFor(c => c.Address)
			.MustBeValueObject(x => Address.Create(x.Country, x.City, x.Street, x.HouseNumber, x.Apartment, x.HouseLiter));
	}
}
