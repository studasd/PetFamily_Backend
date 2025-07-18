using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.VolunteerManagement;
using PetFamily.Contracts.RequestVolonteers;
using PetFamily.Domain.Shared.Errores;
using PetFamily.Domain.SpeciesManagement.Entities;
using PetFamily.Domain.SpeciesManagement.IDs;
using PetFamily.Domain.VolunteerManagement.ValueObjects;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.Repositories;

public class SpeciesRepository(WriteDbContext context) : ISpeciesRepository
{
	private readonly WriteDbContext db = context;


	public async Task<Guid> AddAsync(Species species, CancellationToken token)
	{
		await db.Species.AddAsync(species, token);

		await db.SaveChangesAsync(token);

		return species.Id;
	}

	public async Task<Result<Species, Error>> GetByIdAsync(SpeciesId speciesId, CancellationToken token)
	{
		var species = await db.Species
			.Include(x => x.Breeds)
			.FirstOrDefaultAsync(x => x.Id == speciesId, token);

		if (species == null)
			return Errors.General.NotFound(speciesId);

		return species;
	}

	public async Task<Result<Species, Error>> GetByNameAsync(string speciesName, CancellationToken token)
	{
		var species = await db.Species
			.Include(x => x.Breeds)
			.FirstOrDefaultAsync(x => x.Name == speciesName, token);

		if (species == null)
			return Errors.General.NotFound($"{speciesName}");

		return species;
	}


	public async Task<Result<PetType, Error>> GetPetTypeByNamesAsync(string speciesName, string breedName, CancellationToken token)
	{
		var species = await db.Species
			.Include(x => x.Breeds)
			.FirstOrDefaultAsync(x => x.Name == speciesName, token);

		if (species == null)
			return Errors.General.NotFound($"{speciesName}");

		var breed = species.Breeds.FirstOrDefault(b => b.Name == breedName);
		if (breed == null)
			return Errors.General.NotFound($"{breedName}");

		return PetType.Create(species.Id.Value, breed.Id.Value);
	}


	public async Task<Guid> DeleteAsync(Species species, CancellationToken token)
	{
		db.Species.Remove(species);

		await SaveAsync(token);

		return species.Id;
	}

	public async Task SaveAsync(CancellationToken token)
	{
		await db.SaveChangesAsync(token);
	}

}
