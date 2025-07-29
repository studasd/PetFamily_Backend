using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.PetsManagement.Commands.Add;
using PetFamily.Infrastructure.DbContexts;
using PetFamily.Domain.VolunteerManagement.Entities;
using PetFamily.Domain.VolunteerManagement.IDs;
using PetFamily.Domain.VolunteerManagement.Enums;
using PetFamily.Domain.VolunteerManagement.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.SpeciesManagement.Entities;
using PetFamily.Contracts.DTOs;
using AutoFixture;
using Xunit;

namespace PetFamily.IntegrationTests.PetsManagement;

public class AddPetHandlerTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    private readonly IServiceScope scope;
    private readonly WriteDbContext db;
    private readonly IReadDbContext readDb;
    private readonly ICommandHandler<Guid, AddPetCommand> sut;
    private readonly Fixture fixture;

    public AddPetHandlerTests(IntegrationTestsWebFactory factory)
    {
        scope = factory.Services.CreateScope();
        db = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        readDb = scope.ServiceProvider.GetRequiredService<IReadDbContext>();
        sut = scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, AddPetCommand>>();
        fixture = new Fixture();
    }

    [Fact]
    public async Task HandleAsync_ValidCommand_ShouldAddPetToDatabase()
    {
        // Arrange
        var volunteerId = await SeedVolunteer();
        var (speciesId, breedId) = await SeedSpecies();

        var command = new AddPetCommand(
            volunteerId,
            "TestPet",
            "Test Description", 
            breedId,
            speciesId,
            "Brown",
            10.5m,
            20.5m,
            "79001234567",
            PetHelpStatuses.NeedsHelp,
            new AddressDTO(
                "TestCountry",
                "TestCity",
                "TestStreet",
                1,
                10,
                "A"
            ));

        // Act
        var result = await sut.HandleAsync(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        // Verify the pet was added to the database
        var pet = await readDb.Pets.FirstOrDefaultAsync(p => p.Id == result.Value);
        pet.Should().NotBeNull();
        pet!.Name.Should().Be("TestPet");
        pet.Description.Should().Be("Test Description");
        pet.Color.Should().Be("Brown");
        pet.Weight.Should().Be(10.5m);
        pet.Height.Should().Be(20.5m);
        pet.SpeciesId.Should().Be(speciesId);
        pet.BreedId.Should().Be(breedId);
        pet.VolunteerId.Should().Be(volunteerId);
    }

    [Fact]
    public async Task HandleAsync_InvalidSpeciesId_ShouldReturnError()
    {
        // Arrange
        var volunteerId = await SeedVolunteer();
        var invalidSpeciesId = Guid.NewGuid();
        var invalidBreedId = Guid.NewGuid();

        var command = new AddPetCommand(
            volunteerId,
            "TestPet",
            "Test Description",
            invalidBreedId,
            invalidSpeciesId,
            "Brown",
            10.5m,
            20.5m,
            "79001234567",
            PetHelpStatuses.NeedsHelp,
            new AddressDTO(
                "TestCountry",
                "TestCity",
                "TestStreet",
                1,
                10,
                "A"
            ));

        // Act
        var result = await sut.HandleAsync(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
    }

    private async Task<Guid> SeedVolunteer()
    {
        var volunteer = new Volunteer(
            VolunteerId.NewVolunteerId(),
            VolunteerName.Create("Test", "User", "Userovich").Value,
            "test@example.com",
            "Test Description",
            1,
            Phone.Create("79001234567").Value);

        db.Volunteers.Add(volunteer);
        await db.SaveChangesAsync();

        return volunteer.Id;
    }

    private async Task<(Guid speciesId, Guid breedId)> SeedSpecies()
    {
        var breed = Breed.Create("TestBreed").Value;
        var species = Species.Create("TestSpecies", new[] { breed }).Value;

        db.Species.Add(species);
        await db.SaveChangesAsync();

        return (species.Id, breed.Id);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        // Clean up the database after each test
        if (db != null)
        {
            var volunteers = await db.Volunteers.ToListAsync();
            db.Volunteers.RemoveRange(volunteers);

            var species = await db.Species.ToListAsync();
            db.Species.RemoveRange(species);

            await db.SaveChangesAsync();
        }
        
        scope.Dispose();
    }
}