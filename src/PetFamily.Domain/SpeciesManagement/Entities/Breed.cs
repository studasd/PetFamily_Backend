
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.SpeciesManagement.Entities;

public class Breed : Entity<Guid>
{
	Breed() { }

	public Breed(Guid id, string name) : base(id)
	{
		Name = name;
	}

	public string Name { get; set; }


	public static Guid NewId() => Guid.NewGuid();

	public static Result<Breed, Error> Create(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			return Errors.General.ValueIsRequired("Name");

		var breed = new Breed(NewId(), name);

		return breed;
	}
}