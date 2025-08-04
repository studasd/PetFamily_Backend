using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;

namespace PetFamily.Specieses.Application.Queries.GetSpeciesPagination;

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
