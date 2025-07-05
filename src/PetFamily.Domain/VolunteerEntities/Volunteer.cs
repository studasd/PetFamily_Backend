using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities;
using PetFamily.Domain.PetEntities;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.VolunteerEntities;

public class Volunteer : Entity<VolunteerId>
{
	private Volunteer() { }

	public Volunteer(VolunteerId id, VolunteerName name, string email, string description, int experienceYears, Phone phone, BankingDetails? bankingDetails) : base(id)
	{
		Name = name;
		Email = email;
		Description = description;
		ExperienceYears = experienceYears;
		Phone = phone;
		BankingDetails = bankingDetails ?? new(null, null);
	}

	private readonly List<Pet> _pets = [];

	public VolunteerName Name { get; private set; }
	public string? Email { get; private set; }
	public string? Description { get; private set; }
	public int ExperienceYears { get; private set; }

	public Phone Phone { get; private set; }
	public BankingDetails BankingDetails { get; private set; }
	public SocialNetworkDetails? SocialNetworkDetails { get; private set; } = new();
	public IReadOnlyList<Pet> Pets => _pets;


	public static Guid NewId() => Guid.NewGuid();

	public int GetCountPetFoundHouse() => Pets.Count(p => p.HelpStatus == PetHelpStatuses.FoundHouse);
	public int GetCountPetLookingHome() => Pets.Count(p => p.HelpStatus == PetHelpStatuses.LookingHome);
	public int GetCountPetNeedsHelp() => Pets.Count(p => p.HelpStatus == PetHelpStatuses.NeedsHelp);


	public static Result<Volunteer, Error> Create(VolunteerName volunteerName, string email, string description, int experienceYears, string phone, BankingDetails bankingDetails, IReadOnlyList<SocialNetwork> SocialNetworks)
	{
		if (string.IsNullOrWhiteSpace(email))
			return Errors.General.ValueIsRequired("Email");

		if (string.IsNullOrWhiteSpace(description))
			return Errors.General.ValueIsRequired("Description");

		var ph = Phone.Create(phone);

		if (ph.IsFailure)
			return ph.Error;

		var volunteer = new Volunteer(VolunteerId.NewVolunteerId(), volunteerName, email, description, experienceYears, ph.Value, bankingDetails);

		if(SocialNetworks.Count() > 0)
		{
			foreach (var network in SocialNetworks)
				volunteer.AddSocialNetwork(network.Name, network.Link);
		}

		return volunteer;
	}

	public UnitResult<Error> AddSocialNetwork(string name, string link)
	{
		var socialNetwork = SocialNetwork.Create(name, link);

		if(socialNetwork.IsFailure)
			return socialNetwork.Error;

		SocialNetworkDetails!.Add(socialNetwork.Value);

		return Result.Success<Error>();
	}

	public UnitResult<Error> AddPet(Pet? pet)
	{
		if (pet is null)
			return Errors.General.ValueIsInvalid("Pet");

		_pets.Add(pet);

		return Result.Success<Error>();
	}
}
