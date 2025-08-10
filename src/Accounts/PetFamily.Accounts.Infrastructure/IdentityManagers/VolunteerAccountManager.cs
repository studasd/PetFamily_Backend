using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Application.Abstractions;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class VolunteerAccountManager : IVolunteerAccountManager
{
	private readonly AccountsDbContext accountContext;

	public VolunteerAccountManager(AccountsDbContext context)
	{
		accountContext = context;
	}

	public async Task CreateVolunteerAccount(VolunteerAccount volunteerAccount, CancellationToken token)
	{
		await accountContext.VolunteerAccounts.AddAsync(volunteerAccount, token);
		await accountContext.SaveChangesAsync(token);
	}
}