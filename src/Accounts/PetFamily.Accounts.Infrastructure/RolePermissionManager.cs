using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure;

public class RolePermissionManager
{
	private readonly AccountsDbContext accountContext;

	public RolePermissionManager(AccountsDbContext accountContext)
	{
		this.accountContext = accountContext;
	}

	public async Task AddRangeIfExistAsync(Guid roleId, IEnumerable<string> permissions)
	{
		foreach (var permissionCode in permissions)
		{
			var permission = await accountContext.Permissions
				.FirstOrDefaultAsync(p => p.Code == permissionCode);

			var rolePermissionExists = await accountContext.RolePermissions
				.AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permission!.Id);

			if (rolePermissionExists == true)
				continue;

			accountContext.RolePermissions.Add(new RolePermission
			{
				RoleId = roleId,
				PermissionId = permission!.Id
			});
		}

		await accountContext.SaveChangesAsync();
	}
}
