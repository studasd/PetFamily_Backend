using System.Data;

namespace PetFamily.Core;

public interface IUnitOfWork
{
	Task<IDbTransaction> BeginTransactionAsync(CancellationToken token);

	Task SaveChangesAsync(CancellationToken token);
}
