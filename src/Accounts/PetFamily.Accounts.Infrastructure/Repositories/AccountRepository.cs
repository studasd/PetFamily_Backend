using CSharpFunctionalExtensions;
using PetFamily.Accounts.Application.AccountManagement;
using PetFamily.Accounts.Domain;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
	private readonly AccountsDbContext db;

	public AccountRepository(AccountsDbContext dbContext)
	{
		db = dbContext;
	}

	
	public Task<Result<User, Error>> GetByIdAsync(Guid userId, CancellationToken token)
	{
		throw new NotImplementedException();
	}


	public async Task SaveAsync(CancellationToken token)
	{
		await db.SaveChangesAsync(token);
	}

}
