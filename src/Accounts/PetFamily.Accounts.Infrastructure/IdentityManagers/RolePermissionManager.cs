using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class RolePermissionManager
{
	private readonly AccountsDbContext accountContext;

	public RolePermissionManager(AccountsDbContext accountContext)
	{
		this.accountContext = accountContext;
	}

	public async Task AddRangeIfExistAsync(Guid roleId, IEnumerable<string> permissions, CancellationToken token)
	{
		foreach (var permissionCode in permissions)
		{
			var permission = await accountContext.Permissions
				.FirstOrDefaultAsync(p => p.Code == permissionCode, token);
			if (permission == null)
				throw new ApplicationException($"Permission code {permissionCode} is not found");

			var rolePermissionExists = await accountContext.RolePermissions
				.AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permission!.Id, token);

			if (rolePermissionExists == true)
				continue;

			accountContext.RolePermissions.Add(new RolePermission
			{
				RoleId = roleId,
				PermissionId = permission!.Id
			});
		}

		await accountContext.SaveChangesAsync(token);
	}
}
