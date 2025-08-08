using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class PermissionManager
{
	private readonly AccountsDbContext accountContext;

	public PermissionManager(AccountsDbContext accountContext)
	{
		this.accountContext = accountContext;
	}

	public async Task AddRangeIfExistAsync(IEnumerable<string> permissions)
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

	public async Task<HashSet<string>> GetUserPermissionCodesAsync(Guid userId)
	{
		var permissions = await accountContext.Users
			.Include(u => u.Roles)
			.Where(up => up.Id == userId)
			.SelectMany(up => up.Roles)
			.SelectMany(r => r.RolePermissions)
			.Select(rp => rp.Permission.Code)
			.ToListAsync();

		return permissions.ToHashSet();
	}

	public async Task<Permission?> FindByCodeAsync(string code) =>
		await accountContext.Permissions.FirstOrDefaultAsync(p => p.Code == code);
}
