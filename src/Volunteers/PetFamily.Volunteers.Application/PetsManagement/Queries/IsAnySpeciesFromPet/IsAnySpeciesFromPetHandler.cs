using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.PetsManagement.Queries.IsAnySpeciesFromPet;

public class IsAnySpeciesFromPetHandler : IQueryHandler<bool, IsAnySpeciesFromPetQuery>
{
	private readonly IReadDbContext _db;

	public IsAnySpeciesFromPetHandler(IReadDbContext readDbContext)
	{
		_db = readDbContext;
	}

	public async Task<bool> HandleAsync(IsAnySpeciesFromPetQuery query, CancellationToken token)
	{
		var isAnySpecies = await _db.Pets
				.AnyAsync(p => p.SpeciesId == query.SpeciesId, token);

		return isAnySpecies;
	}
}
