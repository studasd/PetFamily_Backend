using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;

namespace PetFamily.Volunteers.Application.VolunteerManagement.Queries.GetVolunteerWithPagination;

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

