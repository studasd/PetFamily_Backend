using CSharpFunctionalExtensions;
using PetFamily.Accounts.Domain;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Abstractions;

public interface IRefreshSessionManager
{
	void Delete(RefreshSession refreshSession);
	Task<Result<RefreshSession, Error>> GetByRefreshTokenAsync(Guid refreshToken, CancellationToken token);
}