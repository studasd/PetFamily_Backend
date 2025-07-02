using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities;

namespace PetFamily.Domain.PetEntities;

public class Pet : Entity<Guid>
{
	public Pet(Guid id, string name, PetTypes type, string description, string breed, string color, decimal weight, decimal height, IEnumerable<Phone> phones, PetHelpStatuses helpStatus) : base(id)
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
	public string Breed { get; private set; }
	public string Color { get; private set; }
	public string? HealthInfo { get; private set; }
	public Address? Address { get; private set; }
	public decimal Weight { get; private set; }
	public decimal Height { get; private set; }
	public IReadOnlyList<Phone> Phones { get; private set; } = [];
	public bool? IsNeutered { get; private set; }
	public bool? IsVaccinated { get; private set; }
	public DateOnly DateBirth { get; private set; } = default(DateOnly);
	public PetHelpStatuses HelpStatus { get; private set; }
	public BankingDetails? BankingВetails { get; private set; }
	public DateTime DateCreated { get; private set; }


	public static Guid NewId() => Guid.NewGuid();

	public static Result<Pet> Create(string name, PetTypes type, string description, string breed, string color, decimal weight, decimal height, string phone, PetHelpStatuses helpStatus)
	{

		if (string.IsNullOrWhiteSpace(name))
			return Result.Failure<Pet>("Name cannot be empty");

		if (type == default)
			return Result.Failure<Pet>("Pet type is required");

		if (string.IsNullOrWhiteSpace(description))
			return Result.Failure<Pet>("Description cannot be empty");

		if (string.IsNullOrWhiteSpace(breed))
			return Result.Failure<Pet>("Breed cannot be empty");

		if (string.IsNullOrWhiteSpace(color))
			return Result.Failure<Pet>("Color cannot be empty");

		if (weight <= 0 || weight > 100)
			return Result.Failure<Pet>("Weight error");

		if (height <= 0 || height > 100)
			return Result.Failure<Pet>("Height error");

		if (string.IsNullOrWhiteSpace(phone))
			return Result.Failure<Pet>("Phone number  cannot be empty");

		if (helpStatus == default)
			return Result.Failure<Pet>("Pet help status is required");

		var ph = Phone.Create(phone);

		if(ph.IsFailure)
			return Result.Failure<Pet>(ph.Error);

		var pet = new Pet(NewId(), name, type, description, breed, color, weight, height, new List<Phone> { ph.Value }, helpStatus);

		return Result.Success(pet);
	}
}
