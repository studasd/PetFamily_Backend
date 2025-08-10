using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class AdminAccountManager
{
	private readonly AccountsDbContext accountContext;

	public AdminAccountManager(AccountsDbContext accountContext)
	{
		this.accountContext = accountContext;
	}

	public async Task CreateAdminAccountAsync(AdminAccount adminAccount, CancellationToken token)
	{
		await accountContext.AdminAccounts.AddAsync(adminAccount, token);
		await accountContext.SaveChangesAsync(token);
	}
}
