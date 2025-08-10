using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Application.Abstractions;
using PetFamily.Accounts.Domain;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class RefreshSessionManager : IRefreshSessionManager
{
	private readonly AccountsDbContext accountContext;

	public RefreshSessionManager(AccountsDbContext accountContext)
	{
		this.accountContext = accountContext;
	}

	public async Task<Result<RefreshSession, Error>> GetByRefreshTokenAsync(Guid refreshToken, CancellationToken token)
	{
		var refreshSession = await accountContext.RefreshSessions
			.Include(x => x.User)
			.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken, token);

		if (refreshSession == null)
			return Errors.General.NotFound(refreshToken);

		return refreshSession;
	}

	public void Delete(RefreshSession refreshSession)
	{
		accountContext.RefreshSessions.Remove(refreshSession);
	}
}