using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerManagement.Enums;
using PetFamily.Domain.VolunteerManagement.IDs;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Domain.VolunteerManagement.Entities;

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

	public IReadOnlyList<BankingDetails> BankingDetails => bankingDetails;
	private readonly List<BankingDetails> bankingDetails = [];

	public IReadOnlyList<SocialNetwork> SocialNetworks => socialNetworks;
	private readonly List<SocialNetwork> socialNetworks = [];


	public IReadOnlyList<Pet> Pets => _pets;


	public static Guid NewId() => Guid.NewGuid();

	public int GetCountPetFoundHouse() => Pets.Count(p => p.HelpStatus == PetHelpStatuses.FoundHouse);
	public int GetCountPetLookingHome() => Pets.Count(p => p.HelpStatus == PetHelpStatuses.LookingHome);
	public int GetCountPetNeedsHelp() => Pets.Count(p => p.HelpStatus == PetHelpStatuses.NeedsHelp);


	public static Result<Volunteer, Error> Create(VolunteerName volunteerName, string email, string description, int experienceYears, Phone phone)
	{
		var volunteer = new Volunteer(VolunteerId.NewVolunteerId(), volunteerName, email, description, experienceYears, phone);

		return volunteer;
	}

	public UnitResult<Error> AddSocialNetworks(IEnumerable<SocialNetwork> socNetworks)
	{
		socialNetworks.AddRange(socNetworks);

		return Result.Success<Error>();
	}

	public UnitResult<Error> AddBankingDetails(IEnumerable<BankingDetails> bankDetails)
	{
		bankingDetails.AddRange(bankDetails);

		return Result.Success<Error>();
	}

	public void UpdateInfo(VolunteerName name, string email, string description)
	{
		Name = name;
		Email = email;
		Description = description;
	}

	public void UpdateSocialNetworks(IEnumerable<SocialNetwork> socialNetworks)
	{
		this.socialNetworks.Clear();
		this.socialNetworks.AddRange(socialNetworks);
	}
	
	public void UpdateBankingDetails(IEnumerable<BankingDetails> bankingDetails)
	{
		this.bankingDetails.Clear();
		this.bankingDetails.AddRange(bankingDetails);
	}
}
