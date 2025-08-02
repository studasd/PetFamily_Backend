using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.Volunteers.Application;

namespace PetFamily.Volunteers.Application.SpeciesManagemets.Queries.GetSpeciesPagination;

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
