using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Contracts.Volonteers;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerManagement.Entities;
using PetFamily.Domain.VolunteerManagement.IDs;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Infrastructure.Repositories;

public class VolunteerRepository : IVolunteerRepository
{
	private readonly ApplicationDbContext _db;

	public VolunteerRepository(ApplicationDbContext dbContext)
	{
		this._db = dbContext;
	}

	public async Task<Guid> AddAsync(Volunteer volunteer, CancellationToken token = default)
	{
		await _db.Volunteers.AddAsync(volunteer, token);

		await _db.SaveChangesAsync(token);

		return volunteer.Id;
	}

	public async Task<Result<Volunteer, Error>> GetByIdAsync(VolunteerId volunteerId, CancellationToken token = default)
	{
		var volunteer = await _db.Volunteers
			.Include(x => x.Pets)
			.FirstOrDefaultAsync(x => x.Id == volunteerId, token);

		if (volunteer == null)
			return Errors.General.NotFound(volunteerId);

		return volunteer;
	}

	public async Task<Result<Volunteer, Error>> GetByNameAsync(VolunteerName volunteerName, CancellationToken token = default)
	{
		var volunteer = await _db.Volunteers
			.Include(x => x.Pets)
			.FirstOrDefaultAsync(x => x.Name == volunteerName, token);

		if (volunteer == null)
			return Errors.General.NotFound($"{volunteerName.Firstname} {volunteerName.Lastname} {volunteerName.Surname}");

		return volunteer;
	}
}
