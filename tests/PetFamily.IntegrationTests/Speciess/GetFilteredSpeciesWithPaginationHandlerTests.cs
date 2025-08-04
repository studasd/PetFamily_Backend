using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Specieses.Application;
using PetFamily.Specieses.Application.Queries.GetSpeciesPagination;
using PetFamily.Specieses.Domain.Entities;
using PetFamily.Specieses.Infrastructure.DbContexts;

namespace PetFamily.IntegrationTests.Speciess;

public class GetFilteredSpeciesWithPaginationHandlerTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
	private readonly IServiceScope scope;
	private readonly WriteDbContext db;
	private readonly IReadDbContext readDb;
	private readonly GetFilteredSpeciesWithPaginationHandler sut;
	private readonly Fixture fixture;

	public GetFilteredSpeciesWithPaginationHandlerTests(IntegrationTestsWebFactory factory)
	{
		scope = factory.Services.CreateScope();
		db = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
		readDb = scope.ServiceProvider.GetRequiredService<IReadDbContext>();
		sut = scope.ServiceProvider.GetRequiredService<GetFilteredSpeciesWithPaginationHandler>();
		fixture = new Fixture();
	}

	[Fact]
	public async Task Get_species_with_pagination_should_return_expected_species()
	{
		// arrange
		var species1 = Species.Create("Dog", Array.Empty<Breed>()).Value;
		var species2 = Species.Create("Cat", Array.Empty<Breed>()).Value;
		var species3 = Species.Create("Bird", Array.Empty<Breed>()).Value;

		await db.Species.AddRangeAsync(species1, species2, species3);
		await db.SaveChangesAsync();

		var query = new GetFilteredSpeciesWithPaginationQuery(
			Page: 1,
			PageSize: 2
		);

		// act
		var result = await sut.HandleAsync(query, CancellationToken.None);

		// assert
		result.Should().NotBeNull();
		result.Items.Should().HaveCount(2);
		result.TotalCount.Should().Be(3);
		result.Page.Should().Be(1);
		result.PageSize.Should().Be(2);
		result.HasNextPage.Should().BeTrue();
		result.HasPreviousPage.Should().BeFalse();
	}

	public Task InitializeAsync() => Task.CompletedTask;

	public Task DisposeAsync()
	{
		scope.Dispose();
		return Task.CompletedTask;
	}
}