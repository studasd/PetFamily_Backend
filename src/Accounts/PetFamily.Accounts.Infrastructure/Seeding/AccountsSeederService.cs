using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.Accounts.Application.Abstractions;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.IdentityManagers;
using PetFamily.Accounts.Infrastructure.Options;
using PetFamily.Framework;
using PetFamily.SharedKernel.ValueObjects;
using System.Text.Json;
using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Infrastructure.Seeding;

public class AccountsSeederService
{
	private readonly UserManager<User> userManager;
	private readonly RoleManager<Role> roleManager;
	private readonly PermissionManager permissionManager;
	private readonly RolePermissionManager rolePermissionManager;
	private readonly IAccountsManager adminAccountManager;
	private readonly AdminOptions adminOptions;
	private readonly ILogger<AccountsSeeder> logger;

	public AccountsSeederService(
		UserManager<User> userManager,
		RoleManager<Role> roleManager,
		PermissionManager permissionManager,
		RolePermissionManager rolePermissionManager,
		IOptions<AdminOptions> options,
		IAccountsManager adminAccountManager,
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

	public async Task SeedAsync(CancellationToken token)
	{
		logger.LogInformation("Seeding accounts...");

		var json = await File.ReadAllTextAsync(FilePaths.Accounts);

		var seedData = JsonSerializer.Deserialize<RolePermissionOptions>(json, JsonSerializerOptions.Default)
			?? throw new ApplicationException("Failed to deserialize role permission config");

		await SeedPermissionsAsync(seedData, token);

		await SeedRolesAsync(seedData, token);

		await SeedRolePermissionsAsync(seedData, token);
		
		await SeedAdminAccount(token);
	}

	private async Task SeedAdminAccount(CancellationToken token)
	{
		var isAdminAccountExist = await userManager.FindByEmailAsync(adminOptions.Email);
		if (isAdminAccountExist is not null)
			return;

		var adminRole = await roleManager.FindByNameAsync(AdminAccount.ADMIN)
					?? throw new ApplicationException("Admin role not found");

		var adminUserResult = User.CreateAdmin(adminOptions.UserName, adminOptions.Email, adminRole);
		if (adminUserResult.IsFailure)
			throw new ApplicationException($"Failed to create admin user: {adminUserResult.Error}");
		
		var adminUser = adminUserResult.Value;
		await userManager.CreateAsync(adminUser, adminOptions.Password);
		
		var fullNameResult = FullName.Create(adminOptions.UserName, adminOptions.UserName);
		if (fullNameResult.IsFailure)
			throw new ApplicationException($"Failed to create full name: {fullNameResult.Error}");
		
		var adminAccount = new AdminAccount(adminUser, fullNameResult.Value);
		await adminAccountManager.CreateAdminAccountAsync(adminAccount, token);
	}

	private async Task SeedRolePermissionsAsync(RolePermissionOptions seedData, CancellationToken token)
	{
		foreach (var roleName in seedData.Roles.Keys)
		{
			var role = await roleManager.FindByNameAsync(roleName);

			var rolePermissions = seedData.Roles[roleName];

			await rolePermissionManager.AddRangeIfExistAsync(role!.Id, rolePermissions, token);
		}

		logger.LogInformation("Role permissions added to database");
	}

	private async Task SeedRolesAsync(RolePermissionOptions seedData, CancellationToken token)
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


	private async Task SeedPermissionsAsync(RolePermissionOptions seedData, CancellationToken token)
	{
		var permissionsToAdd = seedData.Permissions.SelectMany(pg => pg.Value);

		await permissionManager.AddRangeIfExistAsync(permissionsToAdd, token);

		logger.LogInformation("Permissions added to database");
	}
}
