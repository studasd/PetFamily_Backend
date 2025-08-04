using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.SharedKernel;
using PetFamily.Specieses.Application;
using PetFamily.Specieses.Domain.Entities;
using PetFamily.Specieses.Domain.IDs;
using PetFamily.Specieses.Infrastructure.DbContexts;

namespace PetFamily.Specieses.Infrastructure.Repositories;

public class SpeciesRepository(SpeciesWriteDbContext context) : ISpeciesRepository
{
	private readonly SpeciesWriteDbContext db = context;


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
