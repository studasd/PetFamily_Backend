using PetFamily.Application.Database;
using PetFamily.Application.DTOs;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;
using PetFamily.Core.Abstractions;

namespace PetFamily.Application.SpeciesManagemets.Queries.GetSpeciesPagination;

public class GetFilteredSpeciesWithPaginationHandler : 
	IQueryHandler<PageList<SpeciesDto>, GetFilteredSpeciesWithPaginationQuery>
{
	private readonly IReadDbContext db;

	public GetFilteredSpeciesWithPaginationHandler(IReadDbContext readDbContext)
	{
		db = readDbContext;
	}

	public async Task<PageList<SpeciesDto>> HandleAsync(GetFilteredSpeciesWithPaginationQuery query, CancellationToken token)
	{
		var speciesQuery = db.Species;

		var pets = await speciesQuery
			.ToPagedListAsync(query.Page, query.PageSize, token);

		return pets;
	}
}
