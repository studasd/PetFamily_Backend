
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Volonteers;
using PetFamily.Contracts.Pets;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.SpeciesManagement.Entities;
using PetFamily.Domain.VolunteerManagement.Entities;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.Pets.Create;

public class CreatePetHandler // CreatePetService
{
	private readonly IVolunteerRepository volunteerRepository;
	private readonly ISpeciesRepository speciesRepository;
	private readonly ILogger<CreatePetHandler> logger;

	public CreatePetHandler(
		IVolunteerRepository volunteerRepository, 
		ISpeciesRepository speciesRepository,
		ILogger<CreatePetHandler> logger)
	{
		this.volunteerRepository = volunteerRepository;
		this.speciesRepository = speciesRepository;
		this.logger = logger;
	}

	public async Task<Result<Guid, Error>> HandleAsync(CreatePetRequest request, CancellationToken token = default)
	{
		var volunteer = await volunteerRepository.GetByIdAsync(request.VolunteerId, token);

		if (volunteer.IsFailure)
			return Errors.General.NotFound("Volunteer");

		var petRequest = request.CreatePetDto;
		var addressDto = petRequest.AddressDTO;

		var phone = Phone.Create(petRequest.Phone).Value;
		var address = Address.Create(addressDto.Country, addressDto.City, addressDto.Street, addressDto.HouseNumber, addressDto.Apartment, addressDto.HouseLiter).Value;

		var breed = Breed.Create(request.CreatePetDto.Breed).Value;
		var speciesResult = Species.Create(request.CreatePetDto.Species, [breed]);
		if (speciesResult.IsFailure) 
			return speciesResult.Error;

		await speciesRepository.AddAsync(speciesResult.Value, token);


		var petType = PetType.Create(breed.Id, speciesResult.Value.Id).Value;

		var pet = Pet.Create(petRequest.Name,
			petRequest.Type,
			petRequest.Description,
			petRequest.Color,
			petRequest.Weight,
			petRequest.Height,
			phone,
			petRequest.HelpStatus,
			address,
			petType).Value;

		volunteer.Value.AddPet(pet);

		await volunteerRepository.SaveAsync(token);

		logger.LogInformation("Created pet {petName} with id {petId}", pet.Name, pet.Id);

		return pet.Id.Value;
	}
}
