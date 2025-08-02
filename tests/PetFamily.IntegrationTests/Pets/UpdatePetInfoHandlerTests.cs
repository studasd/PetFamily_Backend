using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.PetsManagement.Commands.UpdateInfo;
using PetFamily.Infrastructure.DbContexts;
using PetFamily.Domain.VolunteerManagement.Entities;
using PetFamily.Domain.VolunteerManagement.IDs;
using PetFamily.Volunteers.Domain.ValueObjects;
using Xunit;
using PetFamily.Application.Database;
using AutoFixture;
using PetFamily.Core.Abstractions;
using PetFamily.Core.ValueObjects;
using PetFamily.Contracts.Enums;
using PetFamily.Volunteers.Domain.SpeciesManagement.Entities;

namespace PetFamily.IntegrationTests.Pets;

public class UpdatePetInfoHandlerTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    private readonly IServiceScope scope;
    private readonly WriteDbContext db;
    private readonly ICommandHandler<Guid, UpdatePetInfoCommand> sut;
	private readonly Fixture fixture;

	public UpdatePetInfoHandlerTests(IntegrationTestsWebFactory factory)
    {
        scope = factory.Services.CreateScope();
        db = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        sut = scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdatePetInfoCommand>>();
		fixture = new Fixture();
		fixture.Customize<DateOnly>(c => c.FromFactory(() => DateOnly.FromDateTime(DateTime.Now)));
	}


	[Fact]
    public async Task HandleAsync_ValidCommand_Should_Update_Pet_Info()
    {
        // arrange
        var volunteerId = await SeedVolunteer();
        var (speciesId, breedId) = await SeedSpecies();
        var petId = await SeedPet(volunteerId, speciesId, breedId, "Pet1");

        var command = fixture.CreateUpdatePetInfoCommand(
            volunteerId, 
            petId,
            speciesId: speciesId,
            breedId: breedId,
			name: "NewName", 
            phone: "79001112233");

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
        pet!.Name.Should().Be("NewName");
        pet.Description.Should().Be("Updated description");
        pet.Color.Should().Be("black");
        pet.Weight.Should().Be(10.0m);
        pet.Height.Should().Be(20.0m);
    }


    [Fact]
    public async Task HandleAsync_PetNotFound_Should_ReturnError()
    {
        var volunteerId = await SeedVolunteer();
        var fakePetId = Guid.NewGuid();

		var command = fixture.CreateUpdatePetInfoCommand(volunteerId, fakePetId);

		var result = await sut.HandleAsync(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
    }


    [Fact]
    public async Task HandleAsync_VolunteerNotFound_Should_ReturnError()
    {
        var (speciesId, breedId) = await SeedSpecies();
        var petId = await SeedPet(await SeedVolunteer(), speciesId, breedId, "Pet1");
        var fakeVolunteerId = Guid.NewGuid();

		var command = fixture.CreateUpdatePetInfoCommand(
            fakeVolunteerId, 
            petId, 
            speciesId: speciesId,
			breedId: breedId);

		var result = await sut.HandleAsync(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
    }

    [Fact]
    public async Task HandleAsync_InvalidPhone_Should_ReturnError()
    {
        var volunteerId = await SeedVolunteer();
        var (speciesId, breedId) = await SeedSpecies();
        var petId = await SeedPet(volunteerId, speciesId, breedId, "Pet1");

		var command = fixture.CreateUpdatePetInfoCommand(volunteerId, petId, speciesId: speciesId,
			breedId: breedId);

		var result = await sut.HandleAsync(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task HandleAsync_PetBelongsToAnotherVolunteer_Should_ReturnError()
    {
        var volunteerId1 = await SeedVolunteer();
        var volunteerId2 = await SeedVolunteer();
        var (speciesId, breedId) = await SeedSpecies();
        var petId = await SeedPet(volunteerId1, speciesId, breedId, "Pet1");

		var command = fixture.CreateUpdatePetInfoCommand(volunteerId2, petId, speciesId: speciesId, breedId: breedId);

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