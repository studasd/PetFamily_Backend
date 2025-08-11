using PetFamily.Accounts.Application.Abstractions;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class AccountsManager : IAccountsManager
{
	private readonly AccountsDbContext accountContext;

	public AccountsManager(AccountsDbContext accountContext)
	{
		this.accountContext = accountContext;
	}

	public async Task CreateAdminAccountAsync(AdminAccount adminAccount, CancellationToken token)
	{
		await accountContext.AdminAccounts.AddAsync(adminAccount, token);
		await accountContext.SaveChangesAsync(token);
	}

	public async Task CreateParticipantAccountAsync(ParticipantAccount participantAccount, CancellationToken token)
	{
		await accountContext.ParticipantAccounts.AddAsync(participantAccount, token);
		await accountContext.SaveChangesAsync(token);
	}

	public async Task CreateVolunteerAccount(VolunteerAccount volunteerAccount, CancellationToken token)
	{
		await accountContext.VolunteerAccounts.AddAsync(volunteerAccount, token);
		await accountContext.SaveChangesAsync(token);
	}
}
