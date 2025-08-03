using System.Data;

namespace PetFamily.Core.Abstractions;

public interface IUnitOfWork
{
	Task<IDbTransaction> BeginTransactionAsync(CancellationToken token);

	Task SaveChangesAsync(CancellationToken token);
}
