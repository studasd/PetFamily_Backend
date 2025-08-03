using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.VolunteerManagement.UseCases.Create;
using PetFamily.Application.VolunteerManagement.UseCases.Updates.BankingDetails;
using PetFamily.Volunteers.Contracts.DTOs;
using PetFamily.Infrastructure.DbContexts;
using PetFamily.Domain.VolunteerManagement.Entities;
using PetFamily.Domain.VolunteerManagement.IDs;
using AutoFixture;
using Xunit;
using PetFamily.Core.Abstractions;

namespace PetFamily.IntegrationTests.Volunteers;

public class UpdateBankingDetailsTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
	private readonly IServiceScope scope;
	private readonly WriteDbContext db;
	private readonly ICommandHandler<Guid, UpdateBankingDetailsCommand> sut;
	private readonly ICommandHandler<Guid, CreateVolunteerCommand> createSut;
	private readonly Fixture fixture;

	public UpdateBankingDetailsTests(IntegrationTestsWebFactory factory)
	{
		scope = factory.Services.CreateScope();
		db = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
		sut = scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdateBankingDetailsCommand>>();
		createSut = scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateVolunteerCommand>>();
		fixture = new Fixture();
	}

	[Fact]
	public async Task Update_banking_details_should_succeed()
	{
		// arrange
		var createCommand = fixture.CreateCreateVolunteerCommand();
		var createResult = await createSut.HandleAsync(createCommand, default);
		createResult.IsSuccess.Should().BeTrue();
		var volunteerId = createResult.Value;

		var newDetails = new List<BankingDetailsDTO>
		{
			new("Test Bank", "Main account"),
			new("Another Bank", "Savings")
		};
		var updateCommand = new UpdateBankingDetailsCommand(volunteerId, newDetails);

		// act
		var updateResult = await sut.HandleAsync(updateCommand, default);

		// assert
		updateResult.IsSuccess.Should().BeTrue();
		var volunteer = await db.Volunteers.FirstOrDefaultAsync(v => v.Id == VolunteerId.Create(volunteerId));
		volunteer.Should().NotBeNull();
		volunteer!.BankingDetails.Should().HaveCount(2);
		volunteer.BankingDetails[0].Name.Should().Be("Test Bank");
		volunteer.BankingDetails[1].Name.Should().Be("Another Bank");
	}

	[Fact]
	public async Task Update_banking_details_to_empty_should_clear_all()
	{
		// arrange
		var createCommand = fixture.CreateCreateVolunteerCommandWithBankingDetails();
		var createResult = await createSut.HandleAsync(createCommand, default);
		createResult.IsSuccess.Should().BeTrue();
		var volunteerId = createResult.Value;

		var updateCommand = new UpdateBankingDetailsCommand(volunteerId, new List<BankingDetailsDTO>());

		// act
		var updateResult = await sut.HandleAsync(updateCommand, default);

		// assert
		updateResult.IsSuccess.Should().BeTrue();
		var volunteer = await db.Volunteers.FirstOrDefaultAsync(v => v.Id == VolunteerId.Create(volunteerId));
		volunteer.Should().NotBeNull();
		volunteer!.BankingDetails.Should().BeEmpty();
	}

	[Fact]
	public async Task Update_banking_details_for_nonexistent_volunteer_should_fail()
	{
		// arrange
		var nonExistentId = Guid.NewGuid();
		var updateCommand = new UpdateBankingDetailsCommand(nonExistentId, new List<BankingDetailsDTO>{ new("Bank", "Desc") });

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