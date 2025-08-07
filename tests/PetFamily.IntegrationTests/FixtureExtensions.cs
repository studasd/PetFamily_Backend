using AutoFixture;
using PetFamily.Volunteers.Application.PetsManagement.Commands.Add;
using PetFamily.Volunteers.Application.PetsManagement.Commands.UpdateInfo;
using PetFamily.Volunteers.Application.PetsManagement.Commands.UpdatePrimePhoto;
using PetFamily.Volunteers.Application.PetsManagement.Commands.UpdateStatus;
using PetFamily.Volunteers.Application.VolunteerManagement.UseCases.Create;
using PetFamily.Volunteers.Contracts.DTOs;
using PetFamily.Volunteers.Contracts.Enums;

namespace PetFamily.IntegrationTests;

public static class FixtureExtensions
{
	public static AddPetCommand CreateAddPetCommand(this IFixture fixture, Guid volunteerId, Guid speciesId, Guid breedId)
	{
		return fixture.Build<AddPetCommand>()
			.With(x => x.VolunteerId, volunteerId)
			.With(x => x.Height, 7)
			.With(x => x.Weight, 15)
			.With(x => x.Phone, Random.Shared.NextInt64(79010101020, 79910101020).ToString())
			.With(x => x.Color, "color")
			.With(x => x.SpeciesId, speciesId)
			.With(x => x.BreedId, breedId)
			.Create();
	}

	public static CreateVolunteerCommand CreateCreateVolunteerCommand(this IFixture fixture)
	{
		var unique = Guid.NewGuid().ToString("N");
		return fixture.Build<CreateVolunteerCommand>()
			.With(x => x.Name, new NameDTO("John", "Doe", "Smith"))
			.With(x => x.Email, $"john.doe.{unique}@example.com")
			.With(x => x.Description, "Test volunteer description")
			.With(x => x.ExperienceYears, 5)
			.With(x => x.Phone, Random.Shared.NextInt64(79010101020, 79910101020).ToString())
			.With(x => x.BankingDetails, new List<BankingDetailsDTO>())
			.Create();
	}

	//public static CreateVolunteerCommand CreateCreateVolunteerCommandWithSocialNetworks(this IFixture fixture)
	//{
	//	var unique = Guid.NewGuid().ToString("N");
	//	var socialNetworks = new List<SocialNetworkDTO>
	//	{
	//		new("Facebook", "https://facebook.com/johndoe"),
	//		new("Instagram", "https://instagram.com/johndoe")
	//	};

	//	return fixture.Build<CreateVolunteerCommand>()
	//		.With(x => x.Name, new NameDTO("Jane", "Smith", "Johnson"))
	//		.With(x => x.Email, $"jane.smith.{unique}@example.com")
	//		.With(x => x.Description, "Test volunteer with social networks")
	//		.With(x => x.ExperienceYears, 3)
	//		.With(x => x.Phone, Random.Shared.NextInt64(79010101020, 79910101020).ToString())
	//		.With(x => x.BankingDetails, new List<BankingDetailsDTO>())
	//		.Create();
	//}

	public static CreateVolunteerCommand CreateCreateVolunteerCommandWithBankingDetails(this IFixture fixture)
	{
		var unique = Guid.NewGuid().ToString("N");
		var bankingDetails = new List<BankingDetailsDTO>
		{
			new("Bank of America", "Checking account"),
			new("Wells Fargo", "Savings account")
		};

		return fixture.Build<CreateVolunteerCommand>()
			.With(x => x.Name, new NameDTO("Bob", "Wilson", "Brown"))
			.With(x => x.Email, $"bob.wilson.{unique}@example.com")
			.With(x => x.Description, "Test volunteer with banking details")
			.With(x => x.ExperienceYears, 7)
			.With(x => x.Phone, Random.Shared.NextInt64(79010101020, 79910101020).ToString())
			.With(x => x.BankingDetails, bankingDetails)
			.Create();
	}

	public static CreateVolunteerCommand CreateCreateVolunteerCommandWithName(this IFixture fixture, string firstname, string lastname, string surname)
	{
		var unique = Guid.NewGuid().ToString("N");
		return fixture.Build<CreateVolunteerCommand>()
			.With(x => x.Name, new NameDTO(firstname, lastname, surname))
			.With(x => x.Email, $"duplicate.name.{unique}@example.com")
			.With(x => x.Description, "Test volunteer with duplicate name")
			.With(x => x.ExperienceYears, 2)
			.With(x => x.Phone, Random.Shared.NextInt64(79010101020, 79910101020).ToString())
			.With(x => x.BankingDetails, new List<BankingDetailsDTO>())
			.Create();
	}

	public static CreateVolunteerCommand CreateCreateVolunteerCommandWithInvalidEmail(this IFixture fixture)
	{
		var unique = Guid.NewGuid().ToString("N");
		return fixture.Build<CreateVolunteerCommand>()
			.With(x => x.Name, new NameDTO("Invalid", "Email", "Test"))
			.With(x => x.Email, "invalid-email")
			.With(x => x.Description, "Test volunteer with invalid email")
			.With(x => x.ExperienceYears, 1)
			.With(x => x.Phone, Random.Shared.NextInt64(79010101020, 79910101020).ToString())
			.With(x => x.BankingDetails, new List<BankingDetailsDTO>())
			.Create();
	}

	public static CreateVolunteerCommand CreateCreateVolunteerCommandWithInvalidPhone(this IFixture fixture)
	{
		var unique = Guid.NewGuid().ToString("N");
		return fixture.Build<CreateVolunteerCommand>()
			.With(x => x.Name, new NameDTO("Invalid", "Phone", "Test"))
			.With(x => x.Email, $"invalid.phone.{unique}@example.com")
			.With(x => x.Description, "Test volunteer with invalid phone")
			.With(x => x.ExperienceYears, 1)
			.With(x => x.Phone, "bad-phone")
			.With(x => x.BankingDetails, new List<BankingDetailsDTO>())
			.Create();
	}

	public static UpdatePetInfoCommand CreateUpdatePetInfoCommand(
		this IFixture fixture,
		Guid volunteerId,
		Guid petId,
		Guid? speciesId = null, 
		Guid? breedId = null,
		string? name = null,
		string? description = null,
		string? color = null,
		decimal? weight = null,
		decimal? height = null,
		string? phone = null)
	{
		return fixture.Build<UpdatePetInfoCommand>()
			.With(x => x.VolunteerId, volunteerId)
			.With(x => x.PetId, petId)
			.With(x => x.SpeciesId, speciesId ?? Guid.NewGuid())
			.With(x => x.BreedId, breedId ?? Guid.NewGuid())
			.With(x => x.Name, name ?? $"Pet_{Guid.NewGuid():N}")
			.With(x => x.Description, description ?? "Updated description")
			.With(x => x.Color, color ?? "black")
			.With(x => x.Weight, weight ?? 10.0m)
			.With(x => x.Height, height ?? 20.0m)
			.With(x => x.Phones, phone is null ? [Random.Shared.NextInt64(79010000000, 79999999999).ToString()] : [phone])
			.Create();
	}

	public static UpdatePetPrimePhotoCommand CreateUpdatePetPrimePhotoCommand(
		this IFixture fixture,
		Guid volunteerId,
		Guid petId,
		string? photoPath = null)
	{
		return fixture.Build<UpdatePetPrimePhotoCommand>()
			.With(x => x.VolunteerId, volunteerId)
			.With(x => x.PetId, petId)
			.With(x => x.PathPhoto, photoPath ?? $"/photos/{Guid.NewGuid():N}.jpg")
			.Create();
	}

	public static UpdatePetStatusCommand CreateUpdatePetStatusCommand(
		this IFixture fixture,
		Guid volunteerId,
		Guid petId,
		PetHelpStatuses? status = null)
	{
		return fixture.Build<UpdatePetStatusCommand>()
			.With(x => x.VolunteerId, volunteerId)
			.With(x => x.PetId, petId)
			.With(x => x.HelpStatus, status ?? PetHelpStatuses.FoundHouse)
			.Create();
	}
}
