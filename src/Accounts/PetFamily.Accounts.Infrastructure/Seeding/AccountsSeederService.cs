using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.IdentityManagers;
using PetFamily.Accounts.Infrastructure.Options;
using PetFamily.Framework;
using PetFamily.SharedKernel.ValueObjects;
using System.Text.Json;

namespace PetFamily.Accounts.Infrastructure.Seeding;

public class AccountsSeederService
{
	private readonly UserManager<User> userManager;
	private readonly RoleManager<Role> roleManager;
	private readonly PermissionManager permissionManager;
	private readonly RolePermissionManager rolePermissionManager;
	private readonly AdminAccountManager adminAccountManager;
	private readonly AdminOptions adminOptions;
	private readonly ILogger<AccountsSeeder> logger;

	public AccountsSeederService(
		UserManager<User> userManager,
		RoleManager<Role> roleManager,
		PermissionManager permissionManager,
		RolePermissionManager rolePermissionManager,
		IOptions<AdminOptions> options,
		AdminAccountManager adminAccountManager,
		ILogger<AccountsSeeder> logger)
	{
		this.userManager = userManager;
		this.roleManager = roleManager;
		this.permissionManager = permissionManager;
		this.rolePermissionManager = rolePermissionManager;
		this.adminAccountManager = adminAccountManager;
		this.adminOptions = options.Value;
		this.logger = logger;
	}

	public async Task SeedAsync()
	{
		logger.LogInformation("Seeding accounts...");

		var json = await File.ReadAllTextAsync(FilePaths.Accounts);

		var seedData = JsonSerializer.Deserialize<RolePermissionOptions>(json, JsonSerializerOptions.Default)
			?? throw new ApplicationException("Failed to deserialize role permission config");

		await SeedPermissionsAsync(seedData);

		await SeedRolesAsync(seedData);

		await SeedRolePermissionsAsync(seedData);
		
		await SeedAdminAccount();
	}

	private async Task SeedAdminAccount()
	{
		var isAdminAccountExist = await userManager.FindByEmailAsync(adminOptions.Email);
		if (isAdminAccountExist is not null)
			return;

		var adminRole = await roleManager.FindByNameAsync(AdminAccount.ADMIN)
					?? throw new ApplicationException("Admin role not found");

		var adminUser = User.CreateAdmin(adminOptions.UserName, adminOptions.Email, adminRole);
		await userManager.CreateAsync(adminUser, adminOptions.Password);
		
		var fullName = FullName.Create(adminOptions.UserName, adminOptions.UserName);
		var adminAccount = new AdminAccount(adminUser, fullName);
		await adminAccountManager.CreateAdminAccountAsync(adminAccount);
	}

	private async Task SeedRolePermissionsAsync(RolePermissionOptions seedData)
	{
		foreach (var roleName in seedData.Roles.Keys)
		{
			var role = await roleManager.FindByNameAsync(roleName);

			var rolePermissions = seedData.Roles[roleName];

			await rolePermissionManager.AddRangeIfExistAsync(role!.Id, rolePermissions);
		}

		logger.LogInformation("Role permissions added to database");
	}

	private async Task SeedRolesAsync(RolePermissionOptions seedData)
	{
		foreach (var roleName in seedData.Roles.Keys)
		{
			var existingRole = await roleManager.FindByNameAsync(roleName);

			if (existingRole is null)
			{
				await roleManager.CreateAsync(new Role { Name = roleName });
			}
		}

		logger.LogInformation("Roles added to database");
	}


	private async Task SeedPermissionsAsync(RolePermissionOptions seedData)
	{
		var permissionsToAdd = seedData.Permissions.SelectMany(pg => pg.Value);

		await permissionManager.AddRangeIfExistAsync(permissionsToAdd);

		logger.LogInformation("Permissions added to database");
	}
}
