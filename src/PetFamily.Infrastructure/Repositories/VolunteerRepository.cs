using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Volunteers;
using PetFamily.Contracts.RequestVolonteers;
using PetFamily.Domain.Shared.Errores;
using PetFamily.Domain.VolunteerManagement.Entities;
using PetFamily.Domain.VolunteerManagement.IDs;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Infrastructure.Repositories;

public class VolunteerRepository : IVolunteerRepository
{
	private readonly ApplicationDbContext db;

	public VolunteerRepository(ApplicationDbContext dbContext)
	{
		this.db = dbContext;
	}

	public async Task<Guid> AddAsync(Volunteer volunteer, CancellationToken token)
	{
		await db.Volunteers.AddAsync(volunteer, token);

		await db.SaveChangesAsync(token);

		return volunteer.Id;
	}

	public async Task<Result<Volunteer, Error>> GetByIdAsync(VolunteerId volunteerId, CancellationToken token)
	{
		var volunteer = await db.Volunteers
			.Include(x => x.Pets)
			.FirstOrDefaultAsync(x => x.Id == volunteerId, token);

		if (volunteer == null)
			return Errors.General.NotFound(volunteerId);

		return volunteer;
	}

	public async Task<Result<Volunteer, Error>> GetByNameAsync(VolunteerName volunteerName, CancellationToken token)
	{
		var volunteer = await db.Volunteers
			.Include(x => x.Pets)
			.FirstOrDefaultAsync(x => x.Name == volunteerName, token);

		if (volunteer == null)
			return Errors.General.NotFound($"{volunteerName.Firstname} {volunteerName.Lastname} {volunteerName.Surname}");

		return volunteer;
	}

	public async Task<Guid> DeleteAsync(Volunteer volunteer, CancellationToken token)
	{
		db.Volunteers.Remove(volunteer);

		await SaveAsync(token);
		
		return volunteer.Id;
	}

	public async Task SaveAsync(CancellationToken token)
	{
		await db.SaveChangesAsync(token);
	}
}
