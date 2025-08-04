using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Application.PetsManagement.Queries.IsAnyBreedFromPet;
using PetFamily.Volunteers.Application.PetsManagement.Queries.IsAnySpeciesFromPet;
using PetFamily.Volunteers.Contracts;

namespace PetFamily.Volunteers.Presentation;

public class VolunteersContract : IVolunteersContract
{
	private readonly IsAnySpeciesFromPetHandler isAnySpeciesFromPetHandler;
	private readonly IsAnyBreedFromPetHandler isAnyBreedFromPetHandler;

	public VolunteersContract(
		IsAnySpeciesFromPetHandler isAnySpeciesFromPetHandler,
		IsAnyBreedFromPetHandler isAnyBreedFromPetHandler
		)
	{
		this.isAnySpeciesFromPetHandler = isAnySpeciesFromPetHandler;
		this.isAnyBreedFromPetHandler = isAnyBreedFromPetHandler;
	}

	public async Task<bool> IsAnyBreedFromPetAsync(Guid breedId, CancellationToken token)
	{
		var query = new IsAnyBreedFromPetQuery(breedId);
		var isAnyBreed = await isAnyBreedFromPetHandler.HandleAsync(query, token);

		return isAnyBreed;
	}

	public async Task<bool> IsAnySpeciesFromPetAsync(Guid speciesId, CancellationToken token)
	{
		var query = new IsAnySpeciesFromPetQuery(speciesId);
		var isAnySpecies = await isAnySpeciesFromPetHandler.HandleAsync(query, token);

		return isAnySpecies;
	}
}
