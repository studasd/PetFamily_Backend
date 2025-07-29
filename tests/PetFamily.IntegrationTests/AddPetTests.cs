using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.PetsManagement.Commands.Add;
using PetFamily.Infrastructure.DbContexts;
using PetFamily.Domain.VolunteerManagement.Entities;
using PetFamily.Domain.VolunteerManagement.IDs;
using PetFamily.Domain.VolunteerManagement.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects;
using AutoFixture;
using PetFamily.Domain.SpeciesManagement.Entities;
using PetFamily.Domain.SpeciesManagement.IDs;

namespace PetFamily.IntegrationTests;

public class AddPetTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
	private readonly IServiceScope scope;
	private readonly WriteDbContext db;
	private readonly ICommandHandler<Guid, AddPetCommand> sut;
	private readonly Fixture fixture;

	public AddPetTests(IntegrationTestsWebFactory factory)
	{
		scope = factory.Services.CreateScope();
		db = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
		sut = scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, AddPetCommand>>();
		this.fixture = new Fixture();
	}

	[Fact]
	public async Task Add_pet_to_database()
	{
		// arange
		var volunteerId = await SeedModule();
		var (speciesId, breedId) = await SeedSpecies();

		var command = fixture.CreateAddPetCommand(volunteerId, speciesId, breedId);
		
		// act
		// AddPetHandler
		var result = await sut.HandleAsync(command, CancellationToken.None);

		// assert
		result.IsSuccess.Should().BeTrue();
		result.Value.Should().NotBeEmpty();

		var volDb = await db.Volunteers.FirstOrDefaultAsync();
		volDb.Should().NotBeNull();

		var pet = volDb.Pets.FirstOrDefault();
		pet.Should().NotBeNull();
	}

	private async Task<Guid> SeedModule()
	{
		var volunteer = new Volunteer(
			VolunteerId.NewVolunteerId(),
			VolunteerName.Create("firstname", "lastname", "surname").Value,
			"email@ma.il",
			"description",
			1,
			Phone.Create("76968897897").Value);

		await db.Volunteers.AddAsync(volunteer);

		await db.SaveChangesAsync();

		return volunteer.Id;
	}


	private async Task<(Guid, Guid)> SeedSpecies()
	{
		var breed = Breed.Create("The German Shepherd").Value;

		var breedDogs = new Breed[]
		{
			breed,
			Breed.Create("French Bulldog").Value,
		};

		var species = Species.Create(
			"dog",
			breedDogs
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