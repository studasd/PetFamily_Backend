namespace PetFamily.Infrastructure;

//public class UnitOfWork : IUnitOfWork
//{
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
//}
