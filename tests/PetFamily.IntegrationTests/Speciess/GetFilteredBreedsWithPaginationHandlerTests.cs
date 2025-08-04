using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Specieses.Application;
using PetFamily.Specieses.Application.Queries.GetBreedsPagination;
using PetFamily.Specieses.Domain.Entities;
using PetFamily.Specieses.Infrastructure.DbContexts;

namespace PetFamily.IntegrationTests.Speciess;

public class GetFilteredBreedsWithPaginationHandlerTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
	private readonly IServiceScope scope;
	private readonly SpeciesWriteDbContext db;
	private readonly IReadDbContext readDb;
	private readonly GetFilteredBreedsWithPaginationHandler sut;
	private readonly Fixture fixture;

	public GetFilteredBreedsWithPaginationHandlerTests(IntegrationTestsWebFactory factory)
	{
		scope = factory.Services.CreateScope();
		db = scope.ServiceProvider.GetRequiredService<SpeciesWriteDbContext>();
		readDb = scope.ServiceProvider.GetRequiredService<IReadDbContext>();
		sut = scope.ServiceProvider.GetRequiredService<GetFilteredBreedsWithPaginationHandler>();
		fixture = new Fixture();
	}

	[Fact]
	public async Task Get_breeds_with_pagination_should_return_expected_breeds()
	{
		// arrange
		var species = Species.Create("TestSpecies", new[] {
			Breed.Create("Breed1").Value,
			Breed.Create("Breed2").Value,
			Breed.Create("Breed3").Value
		}).Value;
		await db.Species.AddAsync(species);
		await db.SaveChangesAsync();

		var query = new GetFilteredBreedsWithPaginationQuery(
			species.Id,
			Page: 1,
			PageSize: 2
		);

		// act
		var result = await sut.HandleAsync(query, CancellationToken.None);

		// assert
		result.Should().NotBeNull();
		result.Items.Should().HaveCount(2);
		result.TotalCount.Should().Be(3);
		result.Items.Select(b => b.Name).Should().Contain(new[] { "Breed3", "Breed2" });
	}

	public Task DisposeAsync()
	{
		scope.Dispose();
		return Task.CompletedTask;
	}

	public Task InitializeAsync() => Task.CompletedTask;
}