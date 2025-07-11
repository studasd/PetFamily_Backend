
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Pets.Add;
using PetFamily.Application.Volonteers;
using PetFamily.Contracts.Pets;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.SpeciesManagement.Entities;
using PetFamily.Domain.SpeciesManagement.IDs;
using PetFamily.Domain.VolunteerManagement.Entities;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.Pets.Create;

public class AddPetHandler // CreatePetService
{
	private readonly IVolunteerRepository volunteerRepository;
	private readonly ISpeciesRepository speciesRepository;
	private readonly ILogger<AddPetHandler> logger;

	public AddPetHandler(
		IVolunteerRepository volunteerRepository, 
		ISpeciesRepository speciesRepository,
		ILogger<AddPetHandler> logger)
	{
		this.volunteerRepository = volunteerRepository;
		this.speciesRepository = speciesRepository;
		this.logger = logger;
	}

	public async Task<Result<Guid, Error>> HandleAsync(AddPetCommand command, CancellationToken token = default)
	{
		var volunteer = await volunteerRepository.GetByIdAsync(command.PetId, token);

		if (volunteer.IsFailure)
			return volunteer.Error;

		var addressDto = command.AddressDTO;

		var phone = Phone.Create(command.Phone).Value;
		var address = Address.Create(addressDto.Country, addressDto.City, addressDto.Street, addressDto.HouseNumber, addressDto.Apartment, addressDto.HouseLiter).Value;

		var breed = Breed.Create(command.Breed);
		if (breed.IsFailure)
			return breed.Error;

		var speciesResult = Species.Create(command.Species, [breed.Value]);
		if (speciesResult.IsFailure)
			return speciesResult.Error;

		var petType = await speciesRepository.GetPetTypeByNamesAsync(command.Species, command.Breed, token);
		if(petType.IsFailure)
			return petType.Error;

		petType = PetType.Create(BreedId.NewBreedId(), Species.NewId());


		var pet = Pet.Create(
			command.Name,
			command.Type,
			command.Description,
			command.Color,
			command.Weight,
			command.Height,
			phone,
			command.HelpStatus,
			address,
			petType.Value).Value;

		volunteer.Value.AddPet(pet);

		await volunteerRepository.SaveAsync(token);

		logger.LogInformation("Created pet {petName} with id {petId}", pet.Name, pet.Id);

		return pet.Id.Value;
	}
}
