using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.DTOs;
using PetFamily.Application.SpeciesManagemets.Commands.Delete;
using PetFamily.Infrastructure.DbContexts;
using Xunit;
using AutoFixture;
using PetFamily.Domain.SpeciesManagement.Entities;

namespace PetFamily.IntegrationTests.Speciess;

public class DeleteBreedHandlerTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    private readonly IServiceScope scope;
    private readonly WriteDbContext db;
    private readonly IReadDbContext readDb;
    private readonly ICommandHandler<Guid, DeleteBreedCommand> sut;
    private readonly Fixture fixture;

    public DeleteBreedHandlerTests(IntegrationTestsWebFactory factory)
    {
        scope = factory.Services.CreateScope();
        db = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
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