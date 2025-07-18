using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Shared.Errores;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerManagement.Enums;
using PetFamily.Domain.VolunteerManagement.IDs;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Domain.VolunteerManagement.Entities;

public class Volunteer : AbsSoftDeletableEntity<VolunteerId>
{
	private Volunteer(VolunteerId id) : base(id) { }

	public Volunteer(VolunteerId id, VolunteerName name, string email, string description, int experienceYears, Phone phone) : base(id)
	{
		Name = name;
		Email = email;
		Description = description;
		ExperienceYears = experienceYears;
		Phone = phone;
	}


	public VolunteerName Name { get; private set; }
	public string? Email { get; private set; }
	public string? Description { get; private set; }
	public int ExperienceYears { get; private set; }

	public Phone Phone { get; private set; }


	public IReadOnlyList<BankingDetails> BankingDetails => bankingDetails;
	private readonly List<BankingDetails> bankingDetails = [];

	public IReadOnlyList<SocialNetwork> SocialNetworks => socialNetworks;
	private readonly List<SocialNetwork> socialNetworks = [];


	public IReadOnlyList<Pet> Pets => pets;
	private readonly List<Pet> pets = [];

	public static Guid NewId() => Guid.NewGuid();

	public int GetCountPetFoundHouse() => Pets.Count(p => p.HelpStatus == PetHelpStatuses.FoundHouse);
	public int GetCountPetLookingHome() => Pets.Count(p => p.HelpStatus == PetHelpStatuses.LookingHome);
	public int GetCountPetNeedsHelp() => Pets.Count(p => p.HelpStatus == PetHelpStatuses.NeedsHelp);
	
	public Result<Pet, Error> GetPetById(PetId id)
	{
		var pet = Pets.FirstOrDefault(p => p.Id == id);
		if (pet == null)
			return Errors.General.NotFound(id);

		return pet;
	}

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

	public UnitResult<Error> AddPet(Pet pet)
	{
		var positionResult = Position.Create(pets.Count + 1);
		if (positionResult.IsFailure)
			return positionResult.Error;

		pet.SetPosition(positionResult.Value);

		pets.Add(pet);

		return Result.Success<Error>();
	}

	public UnitResult<Error> MovePet(Pet pet, Position newPosition)
	{
		var currentPosition = pet.Position;

		if(currentPosition == newPosition || pets.Count == 1)
			return Result.Success<Error>();

		var adjustPosition = AdjustNewPositionIfOutOfRange(newPosition);
		if(adjustPosition.IsFailure)
			return adjustPosition.Error;

		newPosition = adjustPosition.Value;

		var moveResult = MovePetsBetweenPositions(newPosition, currentPosition);
		if(moveResult.IsFailure)
			return moveResult.Error;

		pet.Move(newPosition);

		return Result.Success<Error>();
	}

	private UnitResult<Error> MovePetsBetweenPositions(Position newPosition, Position currentPosition)
	{
		if (newPosition.Value < currentPosition.Value)
		{
			var petToMoves = pets.Where(x => x.Position.Value >= newPosition.Value && x.Position.Value < currentPosition.Value);

			foreach (var petToMove in petToMoves)
			{
				var result = petToMove.MoveForward();
				if (result.IsFailure)
					return result.Error;
			}
		}
		else if (newPosition.Value > currentPosition.Value)
		{
			var petToMoves = pets.Where(x => x.Position.Value > currentPosition.Value && x.Position.Value <= newPosition.Value);

			foreach (var petToMove in petToMoves)
			{
				var result = petToMove.MoveBack();
				if (result.IsFailure)
					return result.Error;
			}
		}

		return Result.Success<Error>();
	}

	private Result<Position, Error> AdjustNewPositionIfOutOfRange(Position newPosition)
	{
		if (newPosition.Value <= pets.Count)
			return newPosition;

		var lastPosition = Position.Create(pets.Count - 1);
		if (lastPosition.IsFailure)
			return lastPosition.Error;

		return lastPosition.Value;
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

	public override void Delete()
	{
		base.Delete();

		foreach(var pet in pets)
			pet.Delete();
	}

	public override void Restore()
	{
		base.Delete();

		foreach (var pet in pets)
			pet.Restore();
	}
}
