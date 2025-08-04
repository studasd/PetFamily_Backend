using Microsoft.EntityFrameworkCore;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Infrastructure.DbContexts;

namespace PetFamily.Volunteers.Infrastructure.Services;

public class DeleteExpiredVolunteerService
{
	private readonly VolunteerWriteDbContext db;

	public DeleteExpiredVolunteerService(VolunteerWriteDbContext db)
	{
		this.db = db;
	}


	public async Task StartAsync(CancellationToken token)
	{
		var dt = DateTime.UtcNow.AddHours(Constants.SOFT_DELETING_HOUR * -1);

		var delResults = await db.Volunteers
			.Include(v => v.Pets)
			.Where(v => 
				v.IsSoftDeleted == true && 
				v.DateDeletion != null &&
				v.DateDeletion <= dt)
			.ToListAsync(token);

		foreach(var delResult in delResults)
		{
			db.Volunteers.Remove(delResult);
		}

		if(delResults.Count() > 0)
			await db.SaveChangesAsync(token);
	}
}
