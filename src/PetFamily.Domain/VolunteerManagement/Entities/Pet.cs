using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.SpeciesManagement.Entities;
using PetFamily.Domain.VolunteerManagement.Enums;
using PetFamily.Domain.VolunteerManagement.IDs;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Domain.VolunteerManagement.Entities;

public class Pet : AbsSoftDeletableEntity<PetId>
{
	private Pet(PetId id) : base(id) { }

	public Pet(
		PetId id, 
		string name, 
		PetTypes type, 
		string description, 
		string color, 
		decimal weight, 
		decimal height, 
		IEnumerable<Phone> phones, 
		PetHelpStatuses helpStatus,
		Address address,
		PetType petType
		) : base(id)
	{
		Name = name;
		Type = type;
		Description = description;
		Breed = breed;
		Color = color;
		Weight = weight;
		Height = height;
		Phones = phones.ToList();
		HelpStatus = helpStatus;
		Address = address;
		PetType = petType;
		DateCreated = DateTime.UtcNow;
	}

	public string Name { get; private set; }
	public PetTypes Type { get; private set; }
	public string Description { get; private set; }
	public string Color { get; private set; }
	public string? HealthInfo { get; private set; }
	public Address Address { get; private set; }
	public decimal Weight { get; private set; }
	public decimal Height { get; private set; }
	public IReadOnlyList<Phone> Phones { get; set; } = [];
	public bool? IsNeutered { get; private set; }
	public bool? IsVaccinated { get; private set; }
	public DateOnly DateBirth { get; private set; } = default;
	public PetHelpStatuses HelpStatus { get; private set; }
	public BankingDetails BankingВetails { get; private set; } = new(null, null);
	public DateTime DateCreated { get; private set; }
	
	public PetType PetType { get; private set; }


	public static Guid NewId() => Guid.NewGuid();

	public static Result<Pet, Error> Create(
		string name, 
		PetTypes type, 
		string description, 
		string color, 
		decimal weight, 
		decimal height, 
		Phone phone, 
		PetHelpStatuses helpStatus,
		Address address,
		PetType petType)
	{
		var pet = new Pet(
			PetId.NewPeetId(), 
			name, 
			type, 
			description, 
			color, 
			weight, 
			height, 
			new List<Phone> { phone }, 
			helpStatus,
			address,
			petType);


		return pet;
	}
}
