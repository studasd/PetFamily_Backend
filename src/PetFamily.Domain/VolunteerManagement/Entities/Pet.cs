using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.SpeciesManagement.Entities;
using PetFamily.Domain.VolunteerManagement.Enums;
using PetFamily.Domain.VolunteerManagement.IDs;

namespace PetFamily.Domain.VolunteerManagement.Entities;

public class Pet : Entity<PetId>
{
	Pet() { }

	public Pet(PetId id, string name, PetTypes type, string description, Breed breed, string color, decimal weight, decimal height, IEnumerable<Phone> phones, PetHelpStatuses helpStatus) : base(id)
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
	public BankingDetails BankingВetails { get; private set; }
	public DateTime DateCreated { get; private set; }
	
	public Breed Breed { get; private set; }
	public Species Species { get; private set; }


	public static Guid NewId() => Guid.NewGuid();

	public static Result<Pet, Error> Create(string name, PetTypes type, string description, Breed breed, string color, decimal weight, decimal height, Phone phone, PetHelpStatuses helpStatus)
	{

		if (string.IsNullOrWhiteSpace(name))
			return Errors.General.ValueIsRequired("Name");

		if (type == default)
			return Errors.General.ValueIsRequired("Pet type");

		if (string.IsNullOrWhiteSpace(description))
			return Errors.General.ValueIsRequired("Description");

		if (string.IsNullOrWhiteSpace(color))
			return Errors.General.ValueIsRequired("Color");

		if (weight <= 0 || weight > 100)
			return Errors.General.ValueIsInvalid("Weight");

		if (height <= 0 || height > 100)
			return Errors.General.ValueIsInvalid("Height");

		if (helpStatus == default)
			return Errors.General.ValueIsRequired("Help status");

		var pet = new Pet(PetId.NewPeetId(), name, type, description, breed, color, weight, height, new List<Phone> { phone }, helpStatus);

		return pet;
	}
}
