using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Database;
using PetFamily.Application.DTOs;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.PetsManagement.Queries.GetPetsWithPagination;

public class GetPetsWithPaginationHandler
{
	private readonly IReadDbContext db;

	public GetPetsWithPaginationHandler(IReadDbContext readDbContext)
	{
		db = readDbContext;
	}

	public async Task<PageList<PetDto>> HandleAsync(GetPetsWithPaginationQuery query, CancellationToken token)
	{
		var petQuery = db.Pets;

		var pets = await petQuery
			.ToPagedListAsync(query.Page, query.PageSize, token);

		return pets;
	}
}
