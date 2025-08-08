using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs;
using PetFamily.Core.Models;
using PetFamily.Volunteers.Application.VolunteerManagement.Queries.GetVolunteerWithPagination;
using PetFamily.Volunteers.Application.VolunteerManagement.UseCases.Create;
using PetFamily.Volunteers.Infrastructure.DbContexts;

namespace PetFamily.IntegrationTests.Volunteers;

public class GetVolunteerByIdTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
	private readonly IServiceScope scope;
	private readonly VolunteerWriteDbContext db;
	private readonly IQueryHandler<PageList<VolunteerDto>, GetVolunteerByIdQuery> sut;
	private readonly ICommandHandler<Guid, CreateVolunteerCommand> createSut;
	private readonly Fixture fixture;

	public GetVolunteerByIdTests(IntegrationTestsWebFactory factory)
	{
		scope = factory.Services.CreateScope();
		db = scope.ServiceProvider.GetRequiredService<VolunteerWriteDbContext>();
		sut = scope.ServiceProvider.GetRequiredService<IQueryHandler<PageList<VolunteerDto>, GetVolunteerByIdQuery>>();
		createSut = scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateVolunteerCommand>>();
		fixture = new Fixture();
	}

	[Fact]
	public async Task Get_volunteer_by_id_should_return_volunteer()
	{
		// arrange
		var createCommand = fixture.CreateCreateVolunteerCommand();
		var createResult = await createSut.HandleAsync(createCommand, default);
		createResult.IsSuccess.Should().BeTrue();
		var volunteerId = createResult.Value;

		var query = new GetVolunteerByIdQuery(volunteerId, 1, 1);

		// act
		var result = await sut.HandleAsync(query, default);

		// assert
		result.Should().NotBeNull();
		result.Items.Should().HaveCount(1);
		var volunteer = result.Items[0];
		volunteer.Id.Should().Be(volunteerId);
		volunteer.Firstname.Should().Be(createCommand.Name.Firstname);
		volunteer.Lastname.Should().Be(createCommand.Name.Lastname);
		volunteer.Surname.Should().Be(createCommand.Name.Surname);
		volunteer.Email.Should().Be(createCommand.Email);
		volunteer.Description.Should().Be(createCommand.Description);
		volunteer.ExperienceYears.Should().Be(createCommand.ExperienceYears);
		volunteer.Phone.Should().Be(createCommand.Phone);
	}

	[Fact]
	public async Task Get_volunteer_by_nonexistent_id_should_return_empty()
	{
		// arrange
		var nonExistentId = Guid.NewGuid();
		var query = new GetVolunteerByIdQuery(nonExistentId, 1, 1);

		// act
		var result = await sut.HandleAsync(query, default);

		// assert
		result.Should().NotBeNull();
		result.Items.Should().BeEmpty();
	}


	public Task DisposeAsync()
	{
		scope.Dispose();
		return Task.CompletedTask;
	}

	public Task InitializeAsync() => Task.CompletedTask;
} 