using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Specieses.Domain.Entities;
using PetFamily.Volunteers.Application.PetsManagement.Commands.Add;
using PetFamily.Volunteers.Domain.Entities;
using PetFamily.Volunteers.Domain.IDs;
using PetFamily.Volunteers.Domain.ValueObjects;
using PetFamily.Volunteers.Infrastructure.DbContexts;

namespace PetFamily.IntegrationTests;

public class AddPetTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
	private readonly IServiceScope _scope;
	private readonly VolunteerWriteDbContext _db;
	private readonly ICommandHandler<Guid, AddPetCommand> _sut;
	private readonly Fixture _fixture;

	public AddPetTests(IntegrationTestsWebFactory factory)
	{
		_scope = factory.Services.CreateScope();
		_db = _scope.ServiceProvider.GetRequiredService<VolunteerWriteDbContext>();
		_sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, AddPetCommand>>();
		_fixture = new Fixture();
	}

	[Fact]
	public async Task Add_pet_to_database()
	{
		// arange
		var volunteerId = await SeedModule();
		var (speciesId, breedId) = await SeedSpecies();

		var command = _fixture.CreateAddPetCommand(volunteerId, speciesId, breedId);
		
		// act
		// AddPetHandler
		var result = await _sut.HandleAsync(command, CancellationToken.None);

		// assert
		result.IsSuccess.Should().BeTrue();
		result.Value.Should().NotBeEmpty();

		var volDb = await _db.Volunteers.FirstOrDefaultAsync();
		volDb.Should().NotBeNull();
		volDb.Id.Value.Should().Be(volunteerId);

		var pet = volDb.Pets.FirstOrDefault();
		pet.Should().NotBeNull();
		pet.Id.Value.Should().Be(result.Value);
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

		await _db.Volunteers.AddAsync(volunteer);

		await _db.SaveChangesAsync();

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

		//await _db.Species.AddAsync(species);

		await _db.SaveChangesAsync();

		return (species.Id, breed.Id);
	}


	public Task DisposeAsync()
	{
		_scope.Dispose();

		return Task.CompletedTask;
	}

	public Task InitializeAsync() => Task.CompletedTask;
}