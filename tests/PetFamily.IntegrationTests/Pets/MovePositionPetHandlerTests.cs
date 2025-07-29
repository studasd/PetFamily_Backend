using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.PetsManagement.Commands.MovePosition;
using PetFamily.Infrastructure.DbContexts;
using PetFamily.Domain.VolunteerManagement.Entities;
using PetFamily.Domain.VolunteerManagement.IDs;
using PetFamily.Domain.VolunteerManagement.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.SpeciesManagement.Entities;
using Xunit;
using PetFamily.Domain.VolunteerManagement.Enums;
using PetFamily.Application.Database;

namespace PetFamily.IntegrationTests.Pets;

public class MovePositionPetHandlerTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    private readonly IServiceScope scope;
    private readonly WriteDbContext db;
    private readonly ICommandHandler<Guid, MovePositionPetCommand> sut;

    public MovePositionPetHandlerTests(IntegrationTestsWebFactory factory)
    {
        scope = factory.Services.CreateScope();
        db = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        sut = scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, MovePositionPetCommand>>();
    }

    [Fact]
    public async Task HandleAsync_ValidCommand_Should_Move_Pet_Position()
    {
        // arrange
        var volunteerId = await SeedVolunteer();
        var (speciesId, breedId) = await SeedSpecies();
        var petId1 = await SeedPet(volunteerId, speciesId, breedId, "Pet1");
        var petId2 = await SeedPet(volunteerId, speciesId, breedId, "Pet2");

        // act: ���������� ������� ������� �� ������ �������
        var command = new MovePositionPetCommand(volunteerId, petId2, 1);
        var result = await sut.HandleAsync(command, CancellationToken.None);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(petId2);

        var volunteer = await db.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Id == VolunteerId.Create(volunteerId));

        volunteer.Should().NotBeNull();
        var pets = volunteer!.Pets;
        pets.Should().HaveCount(2);
        pets.First(p => p.Id == PetId.Create(petId2)).Position.Value.Should().Be(1);
        pets.First(p => p.Id == PetId.Create(petId1)).Position.Value.Should().Be(2);
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

        await db.Volunteers.AddAsync(volunteer);
        await db.SaveChangesAsync();

        return volunteer.Id;
    }

    private async Task<(Guid speciesId, Guid breedId)> SeedSpecies()
    {
        var breed = Breed.Create("TestBreed").Value;
        var species = Species.Create("TestSpecies", new[] { breed }).Value;

        await db.Species.AddAsync(species);
        await db.SaveChangesAsync();

        return (species.Id, breed.Id);
    }

    private async Task<Guid> SeedPet(Guid volunteerId, Guid speciesId, Guid breedId, string name)
    {
        var volunteer = await db.Volunteers.FirstOrDefaultAsync(v => v.Id == VolunteerId.Create(volunteerId));

        var pet = Pet.Create(
            name,
            "Test Description",
            "Brown",
            10.5m,
            20.5m,
            Phone.Create("79001234567").Value,
            PetHelpStatuses.NeedsHelp,
            Address.Create(
                "TestCountry",
                "TestCity",
                "TestStreet",
                1,
                10,
                "A"
            ).Value,
            PetType.Create(breedId, speciesId).Value
        ).Value;

        volunteer!.AddPet(pet);

        await db.SaveChangesAsync();

        return pet.Id;
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync()
    {
        scope.Dispose();
        return Task.CompletedTask;
    }
}