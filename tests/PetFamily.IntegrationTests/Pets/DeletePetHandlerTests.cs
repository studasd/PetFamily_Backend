using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Specieses.Domain.Entities;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Application.PetsManagement.Commands.Delete;
using PetFamily.Volunteers.Contracts.Enums;
using PetFamily.Volunteers.Domain.Entities;
using PetFamily.Volunteers.Domain.IDs;
using PetFamily.Volunteers.Domain.ValueObjects;
using PetFamily.Volunteers.Infrastructure.DbContexts;

namespace PetFamily.IntegrationTests.Pets;

public class DeletePetHandlerTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    private readonly IServiceScope scope;
    private readonly WriteDbContext db;
    private readonly IReadDbContext readDb;
    private readonly ICommandHandler<Guid, DeletePetCommand> sut;

    public DeletePetHandlerTests(IntegrationTestsWebFactory factory)
    {
        scope = factory.Services.CreateScope();
        db = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        readDb = scope.ServiceProvider.GetRequiredService<IReadDbContext>();
        sut = scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, DeletePetCommand>>();
    }

    [Fact]
    public async Task HandleAsync_ValidPetId_Should_Delete_Pet_From_Database()
    {
        // arrange
        var volunteerId = await SeedVolunteer();
        var (speciesId, breedId) = await SeedSpecies();
        var petId = await SeedPet(volunteerId, speciesId, breedId);

        // act
        var command = new DeletePetCommand(volunteerId, petId, false);
        var result = await sut.HandleAsync(command, CancellationToken.None);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(petId);

        var pet = await readDb.Pets.FirstOrDefaultAsync(p => p.Id == petId);
        pet.Should().BeNull();
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

    private async Task<Guid> SeedPet(Guid volunteerId, Guid speciesId, Guid breedId)
    {
		var volunteer = await db.Volunteers.FirstOrDefaultAsync(v => v.Id == VolunteerId.Create(volunteerId));


		var pet = Pet.Create(
            "TestPet",
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
            new PetType(breedId,speciesId)
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