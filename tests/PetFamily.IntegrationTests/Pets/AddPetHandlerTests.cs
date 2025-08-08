using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Specieses.Domain.Entities;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Application.PetsManagement.Commands.Add;
using PetFamily.Volunteers.Contracts.DTOs;
using PetFamily.Volunteers.Contracts.Enums;
using PetFamily.Volunteers.Domain.Entities;
using PetFamily.Volunteers.Domain.IDs;
using PetFamily.Volunteers.Domain.ValueObjects;
using PetFamily.Volunteers.Infrastructure.DbContexts;

namespace PetFamily.IntegrationTests.Pets;

public class AddPetHandlerTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    private readonly IServiceScope _scope;
    private readonly VolunteerWriteDbContext _db;
    private readonly IReadDbContext _readDb;
    private readonly ICommandHandler<Guid, AddPetCommand> _sut;
    private readonly Fixture _fixture;

    public AddPetHandlerTests(IntegrationTestsWebFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _db = _scope.ServiceProvider.GetRequiredService<VolunteerWriteDbContext>();
        _readDb = _scope.ServiceProvider.GetRequiredService<IReadDbContext>();
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, AddPetCommand>>();
        _fixture = new Fixture();
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
        var result = await _sut.HandleAsync(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        // Verify the pet was added to the database
        var pet = await _readDb.Pets.FirstOrDefaultAsync(p => p.Id == result.Value);
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
        var result = await _sut.HandleAsync(command, CancellationToken.None);

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

        _db.Volunteers.Add(volunteer);
        await _db.SaveChangesAsync();

        return volunteer.Id;
    }

    private async Task<(Guid speciesId, Guid breedId)> SeedSpecies()
    {
        var breed = Breed.Create("TestBreed").Value;
        var species = Species.Create("TestSpecies", new[] { breed }).Value;

        //_db.Species.Add(species);
        await _db.SaveChangesAsync();

        return (species.Id, breed.Id);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync()
    {
		_scope.Dispose();
		return Task.CompletedTask;
	}
}