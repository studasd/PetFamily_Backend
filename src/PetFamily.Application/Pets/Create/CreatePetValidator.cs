using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Contracts.Pets;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.SpeciesManagement.Entities;

namespace PetFamily.Application.Pets.Create;

public class CreatePetValidator : AbstractValidator<CreatePetRequest>
{
	public CreatePetValidator()
	{
		RuleFor(c => c.CreatePetDto.Name)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("Name is not empty"));

		RuleFor(c => c.CreatePetDto.Description)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("Description is not empty"))
			.MaximumLength(100).WithError(Errors.General.ValueIsInvalid("Description maximum lenght: 100"));

		RuleFor(c => c.CreatePetDto.Breed)
			.MustBeValueObject(Breed.Create);

		RuleFor(c => c.CreatePetDto.Species)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("Species is not empty"));

		RuleFor(c => c.CreatePetDto.Color)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("Color is not empty"));

		RuleFor(c => c.CreatePetDto.Weight)
			.InclusiveBetween(0, 100).WithError(Errors.General.ValueIsInvalid("Weight years not valid"));

		RuleFor(c => c.CreatePetDto.Height)
			.InclusiveBetween(0, 100).WithError(Errors.General.ValueIsInvalid("Height years not valid"));

		RuleFor(c => c.CreatePetDto.Phone)
			.MustBeValueObject(Phone.Create);

		//if (type == default)
		//	return Errors.General.ValueIsRequired("Pet type");

		//if (helpStatus == default)
		//	return Errors.General.ValueIsRequired("Help status");
	}
}
