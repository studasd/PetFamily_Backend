﻿using Microsoft.Extensions.DependencyInjection;

namespace PetFamily.Accounts.Infrastructure.Seeding;

public class AccountsSeeder
{
	private readonly IServiceScopeFactory serviceScopeFactory;

	public AccountsSeeder(IServiceScopeFactory serviceScopeFactory)
	{
		this.serviceScopeFactory = serviceScopeFactory;
	}

	public async Task SeedAsync()
	{
		using var scope = serviceScopeFactory.CreateScope();

		var service = scope.ServiceProvider.GetRequiredService<AccountsSeederService>();

		await service.SeedAsync();
	}
}
