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
	public BankingDetails BankingВetails { get; private set; }
	public SocialNetworkDetails? SocialNetworkDetails { get; private set; } = new();
	public IReadOnlyList<Pet> Pets => _pets;


	public static Guid NewId() => Guid.NewGuid();

	public int GetCountPetFoundHouse() => Pets.Count(p => p.HelpStatus == PetHelpStatuses.FoundHouse);
	public int GetCountPetLookingHome() => Pets.Count(p => p.HelpStatus == PetHelpStatuses.LookingHome);
	public int GetCountPetNeedsHelp() => Pets.Count(p => p.HelpStatus == PetHelpStatuses.NeedsHelp);


	public static Result<Volunteer, Error> Create(string firstname, string lastname, string surname, string email, string description, int experienceYears, string phone)
	{
		var nameResult = VolunteerName.Create(firstname, lastname, surname);

		if(nameResult.IsFailure)
			return nameResult.Error;


		if (string.IsNullOrWhiteSpace(email))
			return Errors.General.ValueIsRequired("Email");

		if (string.IsNullOrWhiteSpace(description))
			return Errors.General.ValueIsRequired("Description");

		var ph = Phone.Create(phone);

		if (ph.IsFailure)
			return ph.Error;

		var volunteer = new Volunteer(VolunteerId.NewVolunteerId(), nameResult.Value, email, description, experienceYears, ph.Value);

		return volunteer;
	}

	public Result<Result, Error> AddSocialNetwork(string name, string link)
	{
		var socialNetwork = SocialNetwork.Create(name, link);

		if(socialNetwork.IsFailure)
			return socialNetwork.Error;

		SocialNetworkDetails!.Add(socialNetwork.Value);

		return Result.Success();
	}

	public Result<Result, Error> AddPet(Pet? pet)
	{
		if (pet is null)
			return Errors.General.ValueIsInvalid("Pet");

		_pets.Add(pet);

		return Result.Success();
	}
}
