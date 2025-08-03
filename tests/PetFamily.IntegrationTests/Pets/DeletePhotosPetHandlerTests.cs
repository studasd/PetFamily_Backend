using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.PetsManagement.Commands.DeletePhotos;
using PetFamily.Domain.VolunteerManagement.Entities;
using PetFamily.Domain.VolunteerManagement.IDs;
using PetFamily.Volunteers.Domain.ValueObjects;
using Xunit;
using PetFamily.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions;
using PetFamily.Core.ValueObjects;
using PetFamily.Volunteers.Contracts.Enums;
using PetFamily.Volunteers.Domain.SpeciesManagement.Entities;

namespace PetFamily.IntegrationTests.Pets;

public class DeletePhotosPetHandlerTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    private readonly IServiceScope scope;
    private readonly WriteDbContext db;
    private readonly ICommandHandler<Guid, DeletePhotosPetCommand> sut;

    public DeletePhotosPetHandlerTests(IntegrationTestsWebFactory factory)
    {
        scope = factory.Services.CreateScope();
        db = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        sut = scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, DeletePhotosPetCommand>>();
    }

    [Fact]
    public async Task HandleAsync_ValidCommand_ShouldDeletePhotosFromPet()
    {
        // arrange
        var volunteerId = await SeedVolunteer();
        var (speciesId, breedId) = await SeedSpecies();
        var (petId, filePaths) = await SeedPetWithPhotos(volunteerId, speciesId, breedId);

        var filesToDelete = filePaths.Take(1).ToArray();

        var command = new DeletePhotosPetCommand(volunteerId, petId, filesToDelete);

        // act
        var result = await sut.HandleAsync(command, CancellationToken.None);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(petId);

        var volunteer = await db.Volunteers.FirstOrDefaultAsync(x => x.Id == VolunteerId.Create(volunteerId));
		var pet = volunteer.GetPetById(petId).Value;
        pet.Should().NotBeNull();
        pet!.FileStorages.Select(f => f.PathToStorage).Should().NotContain(filesToDelete);
    }

    [Fact]
    public async Task HandleAsync_InvalidPhotoPath_ShouldReturnError()
    {
        // arrange
        var volunteerId = await SeedVolunteer();
        var (speciesId, breedId) = await SeedSpecies();
        var (petId, _) = await SeedPetWithPhotos(volunteerId, speciesId, breedId);

        var invalidFile = "not_exist.jpg";
        var command = new DeletePhotosPetCommand(volunteerId, petId, new[] { invalidFile });

        // act
        var result = await sut.HandleAsync(command, CancellationToken.None);

        // assert
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

    private async Task<(Guid petId, List<string> filePaths)> SeedPetWithPhotos(Guid volunteerId, Guid speciesId, Guid breedId)
    {
        var file1 = FileStorage.Create(Guid.NewGuid(), ".jpg").Value;
        var file2 = FileStorage.Create(Guid.NewGuid(), ".png").Value;

        var pet = Pet.Create(
            "TestPet",
            "Test Description",
            "Brown",
            10.5m,
            20.5m,
            Phone.Create("79001234567").Value,
            PetHelpStatuses.NeedsHelp,
            Address.Create("TestCountry", "TestCity", "TestStreet", 1, 10, "A").Value,
            PetType.Create(breedId, speciesId).Value
		).Value;

        pet.AddPhotos([file1]);
        pet.AddPhotos([file2]);

        var volunteer = await db.Volunteers.FirstOrDefaultAsync(x => x.Id == VolunteerId.Create(volunteerId));

		volunteer!.AddPet(pet);

		await db.SaveChangesAsync();

        return (pet.Id, new List<string> { file1.PathToStorage, file2.PathToStorage });
    }

    public Task InitializeAsync() => Task.CompletedTask;

	public Task DisposeAsync()
	{
		scope.Dispose();
		return Task.CompletedTask;
	}
}