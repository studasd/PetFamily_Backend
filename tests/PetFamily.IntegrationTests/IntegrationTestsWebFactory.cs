using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PetFamily.Application.Database;
using PetFamily.Infrastructure.DbContexts;
using Testcontainers.PostgreSql;

namespace PetFamily.IntegrationTests;

public class IntegrationTestsWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
	private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
		.WithImage("postgres")
		.WithDatabase("pet_family")
		.WithUsername("postgres")
		.WithPassword("postgres")
		.Build();


	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		//base.ConfigureWebHost(builder);

		builder.ConfigureTestServices(ConfigureDefaultServices);
	}


	protected virtual void ConfigureDefaultServices(IServiceCollection services)
	{
		services.RemoveAll<WriteDbContext>();
		services.RemoveAll<ReadDbContext>();

		services.AddScoped<WriteDbContext>(_ => new WriteDbContext(_dbContainer.GetConnectionString()));

		services.AddScoped<IReadDbContext, ReadDbContext>(_ => new ReadDbContext(_dbContainer.GetConnectionString()));
	}


	public async Task InitializeAsync()
	{
		await _dbContainer.StartAsync();

		await using var scope = Services.CreateAsyncScope();

		var dbScope = scope.ServiceProvider.GetRequiredService<WriteDbContext>();

		await dbScope.Database.EnsureCreatedAsync();
	}

	async Task IAsyncLifetime.DisposeAsync()
	{
		await _dbContainer.StopAsync();
		await _dbContainer.DisposeAsync();
	}
}
