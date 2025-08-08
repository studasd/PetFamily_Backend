using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Specieses.Application;
using PetFamily.Specieses.Application.Commands.Delete;
using PetFamily.Specieses.Domain.Entities;
using PetFamily.Specieses.Infrastructure.DbContexts;

namespace PetFamily.IntegrationTests.Speciess;

public class DeleteBreedHandlerTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    private readonly IServiceScope scope;
    private readonly SpeciesWriteDbContext db;
    private readonly IReadDbContext readDb;
    private readonly ICommandHandler<Guid, DeleteBreedCommand> sut;
    private readonly Fixture fixture;

    public DeleteBreedHandlerTests(IntegrationTestsWebFactory factory)
    {
        scope = factory.Services.CreateScope();
        db = scope.ServiceProvider.GetRequiredService<SpeciesWriteDbContext>();
        readDb = scope.ServiceProvider.GetRequiredService<IReadDbContext>();
        sut = scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, DeleteBreedCommand>>();
        fixture = new Fixture();
    }

    [Fact]
    public async Task Delete_breed_should_remove_breed_from_db()
    {
        // arrange
        var (speciesId, breedId) = await SeedSpeciesWithBreeds();

        // act
        var command = new DeleteBreedCommand(speciesId, breedId);
        var result = await sut.HandleAsync(command, CancellationToken.None);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(breedId);

        // breed should not exist in readDb
        var breed = await readDb.Breeds.FirstOrDefaultAsync(b => b.Id == breedId);
        breed.Should().BeNull();

        // species should still exist
        var species = await readDb.Species.FirstOrDefaultAsync(s => s.Id == speciesId);
        species.Should().NotBeNull();
    }

    private async Task<(Guid speciesId, Guid breedId)> SeedSpeciesWithBreeds()
    {
        var breed = Breed.Create("TestBreed").Value;
        var species = Species.Create(
            "TestSpecies",
            new[] { breed, Breed.Create("OtherBreed").Value }
        ).Value;
        await db.Species.AddAsync(species);
        await db.SaveChangesAsync();
        return (species.Id, breed.Id);
    }

    public Task DisposeAsync()
    {
        scope.Dispose();
        return Task.CompletedTask;
    }

    public Task InitializeAsync() => Task.CompletedTask;
} 