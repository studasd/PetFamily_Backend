using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.VolunteerManagement.UseCases.Create;
using PetFamily.Volunteers.Application.VolunteerManagement.UseCases.Delete;
using PetFamily.Volunteers.Domain.IDs;
using PetFamily.Volunteers.Infrastructure.DbContexts;

namespace PetFamily.IntegrationTests.Volunteers;

public class DeleteVolunteerTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
	private readonly IServiceScope scope;
	private readonly VolunteerWriteDbContext db;
	private readonly ICommandHandler<Guid, DeleteVolunteerCommand> sut;
	private readonly ICommandHandler<Guid, CreateVolunteerCommand> createSut;
	private readonly Fixture fixture;

	public DeleteVolunteerTests(IntegrationTestsWebFactory factory)
	{
		scope = factory.Services.CreateScope();
		db = scope.ServiceProvider.GetRequiredService<VolunteerWriteDbContext>();
		sut = scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, DeleteVolunteerCommand>>();
		createSut = scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateVolunteerCommand>>();
		fixture = new Fixture();
	}

	[Fact]
	public async Task Hard_delete_volunteer_should_remove_from_db()
	{
		// arrange
		var createCommand = fixture.CreateCreateVolunteerCommand();
		var createResult = await createSut.HandleAsync(createCommand, default);
		createResult.IsSuccess.Should().BeTrue();
		var volunteerId = createResult.Value;

		// act
		var deleteCommand = new DeleteVolunteerCommand(volunteerId, false);
		var deleteResult = await sut.HandleAsync(deleteCommand, default);

		// assert
		deleteResult.IsSuccess.Should().BeTrue();
		var volunteer = await db.Volunteers.FirstOrDefaultAsync(v => v.Id == VolunteerId.Create(volunteerId));
		volunteer.Should().BeNull();
	}

	[Fact]
	public async Task Soft_delete_volunteer_should_set_deleted_flag()
	{
		// arrange
		var createCommand = fixture.CreateCreateVolunteerCommand();
		var createResult = await createSut.HandleAsync(createCommand, default);
		createResult.IsSuccess.Should().BeTrue();
		var volunteerId = createResult.Value;

		// act
		var deleteCommand = new DeleteVolunteerCommand(volunteerId, true);
		var deleteResult = await sut.HandleAsync(deleteCommand, default);

		// assert
		deleteResult.IsSuccess.Should().BeTrue();
		var volunteer = await db.Volunteers.IgnoreQueryFilters().FirstOrDefaultAsync(v => v.Id == VolunteerId.Create(volunteerId));
		volunteer.Should().NotBeNull();
		volunteer!.IsSoftDeleted.Should().BeTrue();
	}

	[Fact]
	public async Task Delete_nonexistent_volunteer_should_fail()
	{
		// arrange
		var nonExistentId = Guid.NewGuid();
		var deleteCommand = new DeleteVolunteerCommand(nonExistentId, false);

		// act
		var deleteResult = await sut.HandleAsync(deleteCommand, default);

		// assert
		deleteResult.IsFailure.Should().BeTrue();
	}

	public Task DisposeAsync()
	{
		scope.Dispose();
		return Task.CompletedTask;
	}

	public Task InitializeAsync() => Task.CompletedTask;
} 