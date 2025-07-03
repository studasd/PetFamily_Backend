using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities;
using PetFamily.Domain.PetEntities;

namespace PetFamily.Domain.VolunteerEntities;

public class Volunteer : Entity<VolunteerId>
{
	public Volunteer(VolunteerId id, VolunteerName name, string email, string description, int experienceYears, Phone phone) : base(id)
	{
		Name = name;
		Email = email;
		Description = description;
		ExperienceYears = experienceYears;
		Phone = phone;
	}

	private readonly List<Pet> _pets = [];

	public VolunteerName Name { get; private set; }
	public string? Email { get; private set; }
	public string? Description { get; private set; }
	public int ExperienceYears { get; private set; }

	public Phone Phone { get; private set; }
	public BankingDetails? BankingВetails { get; private set; }
	public SocialNetworkDetails? SocialNetworkDetails { get; private set; } = new();
	public IReadOnlyList<Pet> Pets => _pets;


	public static Guid NewId() => Guid.NewGuid();

	public static Result<Volunteer> Create(string firstname, string lastname, string surname, string email, string description, int experienceYears, string phone)
	{
		var nameResult = VolunteerName.Create(firstname, lastname, surname);

		if(nameResult.IsFailure)
			return Result.Failure<Volunteer>(nameResult.Error);


		if (string.IsNullOrWhiteSpace(email))
			return Result.Failure<Volunteer>("Email cannot be empty");

		if (string.IsNullOrWhiteSpace(description))
			return Result.Failure<Volunteer>("Description cannot be empty");

		var ph = Phone.Create(phone);

		if (ph.IsFailure)
			return Result.Failure<Volunteer>(ph.Error);

		var volunteer = new Volunteer(VolunteerId.NewVolunteerId(), nameResult.Value, email, description, experienceYears, ph.Value);

		return Result.Success(volunteer);
	}

	public int GetCountPetFoundHouse() => Pets.Count(p => p.HelpStatus == PetHelpStatuses.FoundHouse);
	public int GetCountPetLookingHome() => Pets.Count(p => p.HelpStatus == PetHelpStatuses.LookingHome);
	public int GetCountPetNeedsHelp() => Pets.Count(p => p.HelpStatus == PetHelpStatuses.NeedsHelp);

	public Result AddSocialNetwork(string name, string link)
	{
		var socialNetwork = SocialNetwork.Create(name, link);

		if(socialNetwork.IsFailure)
			return Result.Failure(socialNetwork.Error);

		SocialNetworkDetails!.SocialNetworks.Add(socialNetwork.Value);

		return Result.Success();
	}

	public Result AddPet(Pet? pet)
	{
		if (pet is null)
			return Result.Failure("Pet cannot be null to add");

		_pets.Add(pet);

		return Result.Success();
	}
}
