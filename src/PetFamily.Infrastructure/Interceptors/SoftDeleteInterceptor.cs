using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerManagement.Entities;

namespace PetFamily.Infrastructure.Interceptors;

public class SoftDeleteInterceptor : SaveChangesInterceptor
{
	public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
		DbContextEventData eventData, 
		InterceptionResult<int> result, 
		CancellationToken token = default)
	{
		if (eventData.Context == null)
			return await base.SavingChangesAsync(eventData, result, token);

		var entries = eventData.Context.ChangeTracker
			.Entries<ISoftDeletable>()
			.Where(e => e.State == EntityState.Deleted);


		foreach (var entry in entries)
		{
			if (entry.Entity is ISoftDeletable item)
			{
				if (item.IsHardDelete == true)
					continue;

				item.Delete(); // Soft Delete
			}
			entry.State = EntityState.Modified;
			//entry.Entity.Delete();
		}

		return await base.SavingChangesAsync(eventData, result, token);
	}
}
