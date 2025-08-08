using CSharpFunctionalExtensions;
using PetFamily.Accounts.Domain;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.AccountManagement;

public interface IAccountRepository
{
	Task<Result<User, Error>> GetByIdAsync(Guid userId, CancellationToken token);
	Task SaveAsync(CancellationToken token);
}