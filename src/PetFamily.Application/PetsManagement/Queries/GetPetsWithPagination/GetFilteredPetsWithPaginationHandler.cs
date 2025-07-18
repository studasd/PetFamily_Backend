using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.DTOs;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.PetsManagement.Queries.GetPetsWithPagination;

public class GetFilteredPetsWithPaginationHandler : IQueryHandler<PageList<PetDto>, GetFilteredPetsWithPaginationQuery>
{
	private readonly IReadDbContext db;

	public GetFilteredPetsWithPaginationHandler(IReadDbContext readDbContext)
	{
		db = readDbContext;
	}

	public async Task<PageList<PetDto>> HandleAsync(
		GetFilteredPetsWithPaginationQuery query, 
		CancellationToken token)
	{
		var petQuery = db.Pets;

		petQuery = petQuery.WhereIf(
			!String.IsNullOrWhiteSpace(query.Name), 
			x => x.Name.Contains(query.Name!));

		petQuery = petQuery.WhereIf(
			query.PositionFrom is not null, 
			x => x.Position >= query.PositionFrom!.Value);

		petQuery = petQuery.WhereIf(
			query.PositionTo is not null, 
			x => x.Position <= query.PositionTo!.Value);


		var pets = await petQuery
			.OrderBy(x => x.Position)
			.ToPagedListAsync(query.Page, query.PageSize, token);

		return pets;
	}
}
