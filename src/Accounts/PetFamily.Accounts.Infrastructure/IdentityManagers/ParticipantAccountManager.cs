using PetFamily.Accounts.Application.Abstractions;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class ParticipantAccountManager : IParticipantAccountManager
{
	private readonly AccountsDbContext accountContext;

	public ParticipantAccountManager(AccountsDbContext context)
	{
		accountContext = context;
	}

	public async Task CreateParticipantAccountAsync(ParticipantAccount participantAccount, CancellationToken token)
	{
		await accountContext.ParticipantAccounts.AddAsync(participantAccount, token);
		await accountContext.SaveChangesAsync(token);
	}
}