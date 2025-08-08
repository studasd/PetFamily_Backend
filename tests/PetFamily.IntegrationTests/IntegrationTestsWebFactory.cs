using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Infrastructure.DbContexts;
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
		services.RemoveAll<VolunteerWriteDbContext>();
		services.RemoveAll<VolunteerReadDbContext>();

		services.AddScoped<VolunteerWriteDbContext>(_ => new VolunteerWriteDbContext(_dbContainer.GetConnectionString()));

		services.AddScoped<IReadDbContext, VolunteerReadDbContext>(_ => new VolunteerReadDbContext(_dbContainer.GetConnectionString()));
	}


	public async Task InitializeAsync()
	{
		await _dbContainer.StartAsync();

		await using var scope = Services.CreateAsyncScope();

		var dbScope = scope.ServiceProvider.GetRequiredService<VolunteerWriteDbContext>();

		await dbScope.Database.EnsureCreatedAsync();
	}

	async Task IAsyncLifetime.DisposeAsync()
	{
		await _dbContainer.StopAsync();
		await _dbContainer.DisposeAsync();
	}
}
