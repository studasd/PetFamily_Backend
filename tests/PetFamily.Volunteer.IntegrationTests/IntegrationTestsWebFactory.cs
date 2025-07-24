using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.PostgreSql;

namespace PetFamily.Volunteer.IntegrationTests;

public class IntegrationTestsWebFactory : WebApplicationFactory<Program>
{
	private readonly PostgreSqlContainer db = new PostgreSqlBuilder()
		.WithImage("postgres")
		.WithDatabase("pet_family")
		.WithUsername("postgres")
		.WithPassword("postgres")
		.Build();

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		base.ConfigureWebHost(builder);
	}
}
