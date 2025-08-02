using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.VolunteerManagement.UseCases.Create;
using PetFamily.Infrastructure.DbContexts;
using PetFamily.Domain.VolunteerManagement.Entities;
using PetFamily.Domain.VolunteerManagement.IDs;
using PetFamily.Domain.VolunteerManagement.ValueObjects;
using AutoFixture;
using PetFamily.Core.Abstractions;
using PetFamily.Core.ValueObjects;

namespace PetFamily.IntegrationTests.Volunteers;

public class CreateVolunteerTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
	private readonly IServiceScope scope;
	private readonly WriteDbContext db;
	private readonly ICommandHandler<Guid, CreateVolunteerCommand> sut;
	private readonly Fixture fixture;

	public CreateVolunteerTests(IntegrationTestsWebFactory factory)
	{
		scope = factory.Services.CreateScope();
		db = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
		sut = scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateVolunteerCommand>>();
		fixture = new Fixture();
	}

	[Fact]
	public async Task Create_volunteer_with_valid_data_should_succeed()
	{
		// arrange
		var command = fixture.CreateCreateVolunteerCommand();

		// act
		var result = await sut.HandleAsync(command, CancellationToken.None);

		// assert
		result.IsSuccess.Should().BeTrue();
		result.Value.Should().NotBeEmpty();

		var volunteer = await db.Volunteers.FirstOrDefaultAsync(v => v.Id == VolunteerId.Create(result.Value));
		volunteer.Should().NotBeNull();
		volunteer!.Name.Firstname.Should().Be(command.Name.Firstname);
		volunteer.Name.Lastname.Should().Be(command.Name.Lastname);
		volunteer.Name.Surname.Should().Be(command.Name.Surname);
		volunteer.Email.Should().Be(command.Email);
		volunteer.Description.Should().Be(command.Description);
		volunteer.ExperienceYears.Should().Be(command.ExperienceYears);
		volunteer.Phone.PhoneNumber.Should().Be(command.Phone);
	}

	[Fact]
	public async Task Create_volunteer_with_social_networks_should_succeed()
	{
		// arrange
		var command = fixture.CreateCreateVolunteerCommandWithSocialNetworks();

		// act
		var result = await sut.HandleAsync(command, CancellationToken.None);

		// assert
		result.IsSuccess.Should().BeTrue();
		result.Value.Should().NotBeEmpty();

		var volunteer = await db.Volunteers.FirstOrDefaultAsync(v => v.Id == VolunteerId.Create(result.Value));
		volunteer.Should().NotBeNull();
		volunteer!.SocialNetworks.Should().HaveCount(command.SocialNetworks.Count());
	}

	[Fact]
	public async Task Create_volunteer_with_banking_details_should_succeed()
	{
		// arrange
		var command = fixture.CreateCreateVolunteerCommandWithBankingDetails();

		// act
		var result = await sut.HandleAsync(command, CancellationToken.None);

		// assert
		result.IsSuccess.Should().BeTrue();
		result.Value.Should().NotBeEmpty();

		var volunteer = await db.Volunteers.FirstOrDefaultAsync(v => v.Id == VolunteerId.Create(result.Value));
		volunteer.Should().NotBeNull();
		volunteer!.BankingDetails.Should().HaveCount(command.BankingDetails.Count());
	}

	[Fact]
	public async Task Create_volunteer_with_duplicate_name_should_fail()
	{
		// arrange
		var existingVolunteer = await SeedVolunteer();
		var command = fixture.CreateCreateVolunteerCommandWithName(
			existingVolunteer.Name.Firstname,
			existingVolunteer.Name.Lastname,
			existingVolunteer.Name.Surname);

		// act
		var result = await sut.HandleAsync(command, CancellationToken.None);

		// assert
		result.IsFailure.Should().BeTrue();
	}

	[Fact]
	public async Task Create_volunteer_with_invalid_email_should_fail()
	{
		// arrange
		var command = fixture.CreateCreateVolunteerCommandWithInvalidEmail();

		// act
		var result = await sut.HandleAsync(command, CancellationToken.None);

		// assert
		result.IsFailure.Should().BeTrue();
	}

	[Fact]
	public async Task Create_volunteer_with_invalid_phone_should_fail()
	{
		// arrange
		var command = fixture.CreateCreateVolunteerCommandWithInvalidPhone();

		// act
		var result = await sut.HandleAsync(command, CancellationToken.None);

		// assert
		result.IsFailure.Should().BeTrue();
	}

	private async Task<Volunteer> SeedVolunteer()
	{
		var volunteer = new Volunteer(
			VolunteerId.NewVolunteerId(),
			VolunteerName.Create("John", "Doe", "Smith").Value,
			"john.doe@example.com",
			"Test volunteer",
			5,
			Phone.Create("1234567890").Value);

		await db.Volunteers.AddAsync(volunteer);
		await db.SaveChangesAsync();

		return volunteer;
	}

	public Task DisposeAsync()
	{
		scope.Dispose();
		return Task.CompletedTask;
	}

	public Task InitializeAsync() => Task.CompletedTask;
} 