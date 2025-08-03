using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.Specieses.Domain.IDs;

namespace PetFamily.Specieses.Domain.Entities;

public class Breed : Entity<BreedId>
{
	private Breed() { }

	private Breed(BreedId id, string name) : base(id)
	{
		Name = name;
	}

	public string Name { get; set; }


	public static Guid NewId() => Guid.NewGuid();

	public static Result<Breed, Error> Create(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			return Errors.General.ValueIsRequired("Name");

		var breed = new Breed(BreedId.NewBreedId(), name);

		return breed;
	}
}