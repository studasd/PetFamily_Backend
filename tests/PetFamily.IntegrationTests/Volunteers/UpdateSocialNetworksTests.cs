using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.VolunteerManagement.UseCases.Create;
using PetFamily.Volunteers.Domain.IDs;
using PetFamily.Volunteers.Infrastructure.DbContexts;

namespace PetFamily.IntegrationTests.Volunteers;

public class UpdateSocialNetworksTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
	//private readonly IServiceScope scope;
	//private readonly VolunteerWriteDbContext db;
	//private readonly ICommandHandler<Guid, UpdateSocialNetworksCommand> sut;
	//private readonly ICommandHandler<Guid, CreateVolunteerCommand> createSut;
	//private readonly Fixture fixture;

	//public UpdateSocialNetworksTests(IntegrationTestsWebFactory factory)
	//{
	//	scope = factory.Services.CreateScope();
	//	db = scope.ServiceProvider.GetRequiredService<VolunteerWriteDbContext>();
	//	sut = scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdateSocialNetworksCommand>>();
	//	createSut = scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateVolunteerCommand>>();
	//	fixture = new Fixture();
	//}

	//[Fact]
	//public async Task Update_social_networks_should_succeed()
	//{
	//	// arrange
	//	var createCommand = fixture.CreateCreateVolunteerCommand();
	//	var createResult = await createSut.HandleAsync(createCommand, default);
	//	createResult.IsSuccess.Should().BeTrue();
	//	var volunteerId = createResult.Value;

	//	var newSocials = new List<SocialNetworkDTO>
	//	{
	//		new("Facebook", "https://facebook.com/testuser"),
	//		new("Instagram", "https://instagram.com/testuser")
	//	};
	//	var updateCommand = new UpdateSocialNetworksCommand(volunteerId, newSocials);

	//	// act
	//	var updateResult = await sut.HandleAsync(updateCommand, default);

	//	// assert
	//	updateResult.IsSuccess.Should().BeTrue();
	//	var volunteer = await db.Volunteers.FirstOrDefaultAsync(v => v.Id == VolunteerId.Create(volunteerId));
	//	volunteer.Should().NotBeNull();
	//	volunteer!.SocialNetworks.Should().HaveCount(2);
	//	volunteer.SocialNetworks[0].Name.Should().Be("Facebook");
	//	volunteer.SocialNetworks[1].Name.Should().Be("Instagram");
	//}

	//[Fact]
	//public async Task Update_social_networks_to_empty_should_clear_all()
	//{
	//	// arrange
	//	var createCommand = fixture.CreateCreateVolunteerCommandWithSocialNetworks();
	//	var createResult = await createSut.HandleAsync(createCommand, default);
	//	createResult.IsSuccess.Should().BeTrue();
	//	var volunteerId = createResult.Value;

	//	var updateCommand = new UpdateSocialNetworksCommand(volunteerId, new List<SocialNetworkDTO>());

	//	// act
	//	var updateResult = await sut.HandleAsync(updateCommand, default);

	//	// assert
	//	updateResult.IsSuccess.Should().BeTrue();
	//	var volunteer = await db.Volunteers.FirstOrDefaultAsync(v => v.Id == VolunteerId.Create(volunteerId));
	//	volunteer.Should().NotBeNull();
	//	volunteer!.SocialNetworks.Should().BeEmpty();
	//}

	//[Fact]
	//public async Task Update_social_networks_for_nonexistent_volunteer_should_fail()
	//{
	//	// arrange
	//	var nonExistentId = Guid.NewGuid();
	//	var updateCommand = new UpdateSocialNetworksCommand(nonExistentId, new List<SocialNetworkDTO>{ new("VK", "https://vk.com/test") });

	//	// act
	//	var updateResult = await sut.HandleAsync(updateCommand, default);

	//	// assert
	//	updateResult.IsFailure.Should().BeTrue();
	//}

	public Task DisposeAsync()
	{
		//scope.Dispose();
		return Task.CompletedTask;
	}

	public Task InitializeAsync() => Task.CompletedTask;
} 