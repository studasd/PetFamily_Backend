using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.VolunteerManagement.UseCases.Create;
using PetFamily.Application.VolunteerManagement.UseCases.Updates.Info;
using PetFamily.Volunteers.Contracts.DTOs;
using PetFamily.Infrastructure.DbContexts;
using PetFamily.Domain.VolunteerManagement.Entities;
using PetFamily.Domain.VolunteerManagement.IDs;
using AutoFixture;
using Xunit;
using PetFamily.Core.Abstractions;

namespace PetFamily.IntegrationTests.Volunteers;

public class UpdateInfoTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
	private readonly IServiceScope scope;
	private readonly WriteDbContext db;
	private readonly ICommandHandler<Guid, UpdateInfoCommand> sut;
	private readonly ICommandHandler<Guid, CreateVolunteerCommand> createSut;
	private readonly Fixture fixture;

	public UpdateInfoTests(IntegrationTestsWebFactory factory)
	{
		scope = factory.Services.CreateScope();
		db = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
		sut = scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdateInfoCommand>>();
		createSut = scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateVolunteerCommand>>();
		fixture = new Fixture();
	}

	[Fact]
	public async Task Update_info_should_succeed()
	{
		// arrange
		var createCommand = fixture.CreateCreateVolunteerCommand();
		var createResult = await createSut.HandleAsync(createCommand, default);
		createResult.IsSuccess.Should().BeTrue();
		var volunteerId = createResult.Value;

		var newName = new NameDTO("Ivan", "Ivanov", "Ivanovich");
		var newEmail = "ivan.ivanov@example.com";
		var newDescription = "Updated description";
		var updateCommand = new UpdateInfoCommand(volunteerId, newName, newEmail, newDescription);

		// act
		var updateResult = await sut.HandleAsync(updateCommand, default);

		// assert
		updateResult.IsSuccess.Should().BeTrue();
		var volunteer = await db.Volunteers.FirstOrDefaultAsync(v => v.Id == VolunteerId.Create(volunteerId));
		volunteer.Should().NotBeNull();
		volunteer!.Name.Firstname.Should().Be("Ivan");
		volunteer.Name.Lastname.Should().Be("Ivanov");
		volunteer.Name.Surname.Should().Be("Ivanovich");
		volunteer.Email.Should().Be(newEmail);
		volunteer.Description.Should().Be(newDescription);
	}

	[Fact]
	public async Task Update_info_for_nonexistent_volunteer_should_fail()
	{
		// arrange
		var nonExistentId = Guid.NewGuid();
		var updateCommand = new UpdateInfoCommand(nonExistentId, new NameDTO("A", "B", "C"), "a@b.c", "desc");

		// act
		var updateResult = await sut.HandleAsync(updateCommand, default);

		// assert
		updateResult.IsFailure.Should().BeTrue();
	}

	[Fact]
	public async Task Update_info_with_invalid_email_should_fail()
	{
		// arrange
		var createCommand = fixture.CreateCreateVolunteerCommand();
		var createResult = await createSut.HandleAsync(createCommand, default);
		createResult.IsSuccess.Should().BeTrue();
		var volunteerId = createResult.Value;

		var updateCommand = new UpdateInfoCommand(volunteerId, new NameDTO("Test", "Test", "Test"), "invalid-email", "desc");

		// act
		var updateResult = await sut.HandleAsync(updateCommand, default);

		// assert
		updateResult.IsFailure.Should().BeTrue();
	}

	public Task DisposeAsync()
	{
		scope.Dispose();
		return Task.CompletedTask;
	}

	public Task InitializeAsync() => Task.CompletedTask;
} 