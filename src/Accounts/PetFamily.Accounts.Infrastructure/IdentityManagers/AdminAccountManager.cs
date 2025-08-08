using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class AdminAccountManager
{
	private readonly AccountsDbContext accountContext;

	public AdminAccountManager(AccountsDbContext accountContext)
	{
		this.accountContext = accountContext;
	}

	public async Task CreateAdminAccountAsync(AdminAccount adminAccount)
	{
		await accountContext.AdminAccounts.AddAsync(adminAccount);
		await accountContext.SaveChangesAsync();
	}
}
