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

	public async Task<PageList<PetDto>> HandleAsync(GetFilteredPetsWithPaginationQuery query, CancellationToken token)
	{
		var petQuery = db.Pets;

		if (!String.IsNullOrWhiteSpace(query.Name))
		{
			petQuery = petQuery.Where(x => x.Name.Contains(query.Name));
		}

		var pets = await petQuery
			.ToPagedListAsync(query.Page, query.PageSize, token);

		return pets;
	}
}
