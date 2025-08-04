using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.PetsManagement.Queries.IsAnyBreedFromPet;

public class IsAnyBreedFromPetHandler : IQueryHandler<bool, IsAnyBreedFromPetQuery>
{
	private readonly IReadDbContext _db;

	public IsAnyBreedFromPetHandler(IReadDbContext readDbContext)
	{
		_db = readDbContext;
	}

	public async Task<bool> HandleAsync(IsAnyBreedFromPetQuery query, CancellationToken token)
	{
		var isAnyBreed = await _db.Pets
				.AnyAsync(p => p.BreedId == query.BreedId, token);

		return isAnyBreed;
	}
}
