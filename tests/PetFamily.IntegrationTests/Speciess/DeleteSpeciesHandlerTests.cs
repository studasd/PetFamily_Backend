using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Database;
using PetFamily.Infrastructure.DbContexts;
using Xunit;
using AutoFixture;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.SpeciesManagemets.Commands.Delete;
using PetFamily.Volunteers.Domain.SpeciesManagement.Entities;

namespace PetFamily.IntegrationTests.Speciess;

public class DeleteSpeciesHandlerTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    private readonly IServiceScope scope;
    private readonly WriteDbContext db;
    private readonly IReadDbContext readDb;
    private readonly ICommandHandler<Guid, DeleteSpeciesCommand> sut;
    private readonly Fixture fixture;

    public DeleteSpeciesHandlerTests(IntegrationTestsWebFactory factory)
    {
        scope = factory.Services.CreateScope();
        db = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        readDb = scope.ServiceProvider.GetRequiredService<IReadDbContext>();
        sut = scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, DeleteSpeciesCommand>>();
        fixture = new Fixture();
    }

    [Fact]
    public async Task Delete_species_should_remove_species_from_db()
    {
        // arrange
        var speciesId = await SeedSpecies();

        // act
        var command = new DeleteSpeciesCommand(speciesId);
        var result = await sut.HandleAsync(command, CancellationToken.None);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(speciesId);

        // species should not exist in readDb
        var species = await readDb.Species.FirstOrDefaultAsync(s => s.Id == speciesId);
        species.Should().BeNull();
    }

    private async Task<Guid> SeedSpecies()
    {
        var species = Species.Create("TestSpecies", Array.Empty<Breed>()).Value;
        await db.Species.AddAsync(species);
        await db.SaveChangesAsync();
        return species.Id;
    }

    public Task DisposeAsync()
    {
        scope.Dispose();
        return Task.CompletedTask;
    }

    public Task InitializeAsync() => Task.CompletedTask;
}