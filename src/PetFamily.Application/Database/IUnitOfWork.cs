using System.Data;

namespace PetFamily.Application.Database;

public interface IUnitOfWork
{
	Task<IDbTransaction> BeginTransactionAsync(CancellationToken token);

	Task SaveChangesAsync(CancellationToken token);
}
