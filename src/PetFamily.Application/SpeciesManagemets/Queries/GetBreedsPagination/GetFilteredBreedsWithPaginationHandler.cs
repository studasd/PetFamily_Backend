using PetFamily.Application.Database;
using PetFamily.Application.DTOs;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;
using PetFamily.Core.Abstractions;

namespace PetFamily.Application.SpeciesManagemets.Queries.GetBreedsPagination;

public class GetFilteredBreedsWithPaginationHandler :
	IQueryHandler<PageList<BreedDto>, GetFilteredBreedsWithPaginationQuery>
{
	private readonly IReadDbContext db;

	public GetFilteredBreedsWithPaginationHandler(IReadDbContext readDbContext)
	{
		db = readDbContext;
	}

	public async Task<PageList<BreedDto>> HandleAsync(GetFilteredBreedsWithPaginationQuery query, CancellationToken token)
	{
		var breedQuery = db.Breeds;

		breedQuery = breedQuery.Where(x => x.SpeciesId == query.SpeciesId);

		var breeds = await breedQuery
			.ToPagedListAsync(query.Page, query.PageSize, token);

		return breeds;
	}
}
