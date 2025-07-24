using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Database;
using PetFamily.Infrastructure.DbContexts;
using Testcontainers.PostgreSql;

namespace PetFamily.Volunteer.IntegrationTests;

public class IntegrationTestsWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
	private readonly PostgreSqlContainer dbContainer = new PostgreSqlBuilder()
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
		var writeContext = services.SingleOrDefault(s => s.ServiceType == typeof(WriteDbContext));

		var readContext = services.SingleOrDefault(s => s.ServiceType == typeof(ReadDbContext));

		if(writeContext is not null)
			services.Remove(writeContext);

		if(readContext is not null)
			services.Remove(readContext);

		services.AddScoped<WriteDbContext>(_ => new WriteDbContext(dbContainer.GetConnectionString()));

		services.AddScoped<IReadDbContext, ReadDbContext>(_ => new ReadDbContext(dbContainer.GetConnectionString()));
	}


	public async Task InitializeAsync()
	{
		await dbContainer.StartAsync();

		await using var scope = Services.CreateAsyncScope();

		var dbScope = scope.ServiceProvider.GetRequiredService<WriteDbContext>();

		await dbScope.Database.EnsureCreatedAsync();
	}

	async Task IAsyncLifetime.DisposeAsync()
	{
		await dbContainer.StopAsync();
		await dbContainer.DisposeAsync();
	}
}
