using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Specieses.Domain.Entities;
using PetFamily.Volunteers.Application.PetsManagement.Commands.UpdatePrimePhoto;
using PetFamily.Volunteers.Contracts.Enums;
using PetFamily.Volunteers.Domain.Entities;
using PetFamily.Volunteers.Domain.IDs;
using PetFamily.Volunteers.Domain.ValueObjects;
using PetFamily.Volunteers.Infrastructure.DbContexts;

namespace PetFamily.IntegrationTests.Pets;

public class UpdatePetPrimePhotoHandlerTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    private readonly IServiceScope scope;
    private readonly WriteDbContext db;
    private readonly ICommandHandler<Guid, UpdatePetPrimePhotoCommand> sut;
    private readonly Fixture fixture;

    public UpdatePetPrimePhotoHandlerTests(IntegrationTestsWebFactory factory)
    {
        scope = factory.Services.CreateScope();
        db = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        sut = scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdatePetPrimePhotoCommand>>();
        fixture = new Fixture();
        fixture.Customize<DateOnly>(c => c.FromFactory(() => DateOnly.FromDateTime(DateTime.Now)));
    }

    [Fact]
    public async Task HandleAsync_ValidCommand_Should_Update_Prime_Photo()
    {
        // arrange
        var volunteerId = await SeedVolunteer();
        var (speciesId, breedId) = await SeedSpecies();
        var petId = await SeedPet(volunteerId, speciesId, breedId, "Pet1");
        var newPhotoPath = "newprimephoto.jpg";

        var command = fixture.CreateUpdatePetPrimePhotoCommand(volunteerId, petId, newPhotoPath);

        // act
        var result = await sut.HandleAsync(command, CancellationToken.None);

        // assert
        result.IsSuccess.Should().BeTrue();

        var pet = await db.Volunteers
            .Include(v => v.Pets)
            .Where(v => v.Id == VolunteerId.Create(volunteerId))
            .SelectMany(v => v.Pets)
            .FirstOrDefaultAsync(p => p.Id == PetId.Create(petId));
        pet.Should().NotBeNull();
        pet!.FileStorages.Should().Contain(FileStorage.Create(newPhotoPath).Value);
    }

    [Fact]
    public async Task HandleAsync_PetNotFound_Should_ReturnError()
    {
        var volunteerId = await SeedVolunteer();
        var fakePetId = Guid.NewGuid();

        var command = fixture.CreateUpdatePetPrimePhotoCommand(volunteerId, fakePetId);

        var result = await sut.HandleAsync(command, CancellationToken.None);

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

        await db.Volunteers.AddAsync(volunteer);
        await db.SaveChangesAsync();

        return volunteer.Id;
    }

    private async Task<(Guid speciesId, Guid breedId)> SeedSpecies()
    {
        var breed = Breed.Create("TestBreed").Value;
        var species = Species.Create("TestSpecies", new[] { breed }).Value;

        //await db.Species.AddAsync(species);
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
            new PetType(breedId, speciesId)
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