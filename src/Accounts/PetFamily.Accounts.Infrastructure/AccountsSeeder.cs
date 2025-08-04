using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain;
using PetFamily.Framework;
using System.Text.Json;

namespace PetFamily.Accounts.Infrastructure;

public class AccountsSeeder
{
	private readonly IServiceScopeFactory serviceScopeFactory;
	private readonly ILogger<AccountsSeeder> logger;

	public AccountsSeeder(
		IServiceScopeFactory serviceScopeFactory,
		ILogger<AccountsSeeder> logger)
	{
		this.serviceScopeFactory = serviceScopeFactory;
		this.logger = logger;
	}

	public async Task SeedAsync()
	{
		logger.LogInformation("Seeding accounts...");

		var json = await File.ReadAllTextAsync(FilePaths.Accounts);

		using var scope = serviceScopeFactory.CreateScope();

		var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
		var permissionManager = scope.ServiceProvider.GetRequiredService<PermissionManager>();

		var seedData = JsonSerializer.Deserialize<RolePermissionConfig>(json, JsonSerializerOptions.Default)
			?? throw new ApplicationException("Failed to deserialize role permission config");

		await SeedPermissions(permissionManager, seedData);

	}

	private async Task SeedPermissions(PermissionManager permissionManager, RolePermissionConfig seedData)
	{
		var permissionsToAdd = seedData.Permissions.SelectMany(pg => pg.Value);

		await permissionManager.AddIfExist(permissionsToAdd);

		logger.LogInformation("Permissions added to database");
	}

}

public class RolePermissionConfig
{
	public Dictionary<string, string[]> Permissions { get; set; } = [];
	public Dictionary<string, string[]> Roles { get; set; } = [];
}