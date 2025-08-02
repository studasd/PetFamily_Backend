using PetFamily.Application.Database;
using PetFamily.Application.DTOs;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;
using PetFamily.Core.Abstractions;

namespace PetFamily.Application.VolunteerManagement.Queries.GetVolunteerWithPagination;

public class GetVolunteerByIdHandler : IQueryHandler<PageList<VolunteerDto>, GetVolunteerByIdQuery>
{
	private readonly IReadDbContext db;

	public GetVolunteerByIdHandler(IReadDbContext readDbContext)
	{
		db = readDbContext;
	}

	public async Task<PageList<VolunteerDto>> HandleAsync(GetVolunteerByIdQuery query, CancellationToken token)
	{
		var volunteerQuery = db.Volunteers;

		var pets = await volunteerQuery
			.ToPagedListAsync(query.Page, query.PageSize, token);

		return pets;
	}
}

