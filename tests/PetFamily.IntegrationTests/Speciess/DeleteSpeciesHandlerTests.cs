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

public class DeleteSpeciesHandlerTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    private readonly IServiceScope scope;
    private readonly SpeciesWriteDbContext db;
    private readonly IReadDbContext readDb;
    private readonly ICommandHandler<Guid, DeleteSpeciesCommand> sut;
    private readonly Fixture fixture;

    public DeleteSpeciesHandlerTests(IntegrationTestsWebFactory factory)
    {
        scope = factory.Services.CreateScope();
        db = scope.ServiceProvider.GetRequiredService<SpeciesWriteDbContext>();
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