using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure;

public class PermissionManager
{
	private readonly AccountsDbContext accountContext;

	public PermissionManager(AccountsDbContext accountContext)
	{
		this.accountContext = accountContext;
	}

	public async Task AddIfExist(IEnumerable<string> permissions)
	{
		foreach (var permissionCode in permissions)
		{
			var isPermissionExists = await accountContext.Permissions
				.AnyAsync(p => p.Code == permissionCode);

			if (isPermissionExists == true)
				continue;

			await accountContext.Permissions.AddAsync(new Permission { Code = permissionCode });
		}

		await accountContext.SaveChangesAsync();
	}
}
