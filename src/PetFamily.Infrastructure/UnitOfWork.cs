using PetFamily.Core.Abstractions;
using System.Data;

namespace PetFamily.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
	//	private readonly WriteDbContext db;

	//	public UnitOfWork(WriteDbContext db)
	//	{
	//		this.db = db;
	//	}


	//	public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken token)
	//	{
	//		var transaction = await db.Database.BeginTransactionAsync(token);

	//		return transaction.GetDbTransaction();
	//	}

	//	public async Task SaveChangesAsync(CancellationToken token)
	//	{
	//		await db.SaveChangesAsync(token);
	//	}

	public Task<IDbTransaction> BeginTransactionAsync(CancellationToken token)
	{
		throw new NotImplementedException();
	}

	public Task SaveChangesAsync(CancellationToken token)
	{
		throw new NotImplementedException();
	}
}
