using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Database;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Volunteer.IntegrationTests;

public class UnitTest1 : IClassFixture<IntegrationTestsWebFactory>
{
	private readonly IntegrationTestsWebFactory factory;

	public UnitTest1(IntegrationTestsWebFactory factory)
	{
		this.factory = factory;
	}


	[Fact]
	public async Task Test1()
	{
		using var scope = factory.Services.CreateAsyncScope();
		var db = scope.ServiceProvider.GetRequiredService<IReadDbContext>();

		var voluns = await db.Volunteers.ToListAsync();

		voluns.Should().NotBeEmpty();
	}
}