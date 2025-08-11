using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Core.Abstractions;
using System.Data;

namespace PetFamily.Accounts.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
	private readonly AccountsDbContext db;

	public UnitOfWork(AccountsDbContext db)
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
