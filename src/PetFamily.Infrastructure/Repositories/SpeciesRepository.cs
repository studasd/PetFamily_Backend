using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Contracts.Volonteers;
using PetFamily.Domain.Shared;
using PetFamily.Domain.SpeciesManagement.Entities;
using PetFamily.Domain.SpeciesManagement.IDs;

namespace PetFamily.Infrastructure.Repositories;

public class SpeciesRepository(ApplicationDbContext context) : ISpeciesRepository
{
	private readonly ApplicationDbContext db = context;


	public async Task<Guid> AddAsync(Species species, CancellationToken token = default)
	{
		await db.Species.AddAsync(species, token);

		await db.SaveChangesAsync(token);

		return species.Id;
	}

	public async Task<Result<Species, Error>> GetByIdAsync(SpeciesId speciesId, CancellationToken token = default)
	{
		var species = await db.Species
			.Include(x => x.Breeds)
			.FirstOrDefaultAsync(x => x.Id == speciesId, token);

		if (species == null)
			return Errors.General.NotFound(speciesId);

		return species;
	}

	public async Task<Result<Species, Error>> GetByNameAsync(string speciesName, CancellationToken token = default)
	{
		var species = await db.Species
			.Include(x => x.Breeds)
			.FirstOrDefaultAsync(x => x.Name == speciesName, token);

		if (species == null)
			return Errors.General.NotFound($"{speciesName}");

		return species;
	}

	public async Task<Guid> DeleteAsync(Species species, CancellationToken token = default)
	{
		db.Species.Remove(species);

		await SaveAsync(token);

		return species.Id;
	}

	public async Task SaveAsync(CancellationToken token = default)
	{
		await db.SaveChangesAsync(token);
	}

}
