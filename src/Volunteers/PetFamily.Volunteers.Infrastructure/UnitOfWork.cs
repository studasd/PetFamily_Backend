using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Infrastructure.DbContexts;
using System.Data;

namespace PetFamily.Volunteers.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
	private readonly VolunteerWriteDbContext db;

	public UnitOfWork(VolunteerWriteDbContext db)
	{
		this.db = db;
	}


	public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken token)
	{
		var transaction = await db.Database.BeginTransactionAsync(token);

		return transaction.GetDbTransaction();
	}

	public async Task SaveChangesAsync(CancellationToken token)
	{
		await db.SaveChangesAsync(token);
	}
}
