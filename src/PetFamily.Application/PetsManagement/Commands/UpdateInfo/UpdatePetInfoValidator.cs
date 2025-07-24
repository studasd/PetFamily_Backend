using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared.Errores;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.PetsManagement.Commands.UpdateInfo;

public class UpdatePetInfoValidator : AbstractValidator<UpdatePetInfoCommand>
{
	public UpdatePetInfoValidator()
	{
		RuleFor(c => c.VolunteerId)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("VolunteerId is not empty"));

		RuleFor(c => c.PetId)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("PetId is not empty"));

		RuleFor(c => c.SpeciesId)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("SpeciesId is not empty"));

		RuleFor(c => c.BreedId)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("BreedId is not empty"));

		RuleFor(c => c.Name)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("Name is not empty"));

		RuleFor(c => c.Description)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("Description is not empty"))
			.MaximumLength(100).WithError(Errors.General.ValueIsInvalid("Description maximum lenght: 100"));

		RuleFor(c => c.Color)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("Color is not empty"));

		RuleFor(c => c.Weight)
			.InclusiveBetween(0, 100).WithError(Errors.General.ValueIsInvalid("Weight years not valid"));

		RuleFor(c => c.Height)
			.InclusiveBetween(0, 100).WithError(Errors.General.ValueIsInvalid("Height years not valid"));

		RuleForEach(c => c.Phones)
			.MustBeValueObject(Phone.Create);

		RuleFor(c => c.Address)
			.MustBeValueObject(x => Address.Create(x.Country, x.City, x.Street, x.HouseNumber, x.Apartment, x.HouseLiter));

		RuleFor(c => c.BankingВetails)
			.MustBeValueObject(x => BankingDetails.Create(x.Name, x.Description));
	}
}
