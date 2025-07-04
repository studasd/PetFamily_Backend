
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.PetEntities;

public class Breed : Entity<Guid>
{
	Breed() { }

	public Breed(Guid id, string name) : base(id)
	{
		Name = name;
	}

	public string Name { get; set; }


	public static Guid NewId() => Guid.NewGuid();

	public static Result<Breed> Create(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			return Result.Failure<Breed>("Name cannot be empty");

		var breed = new Breed(NewId(), name);

		return Result.Success(breed);
	}
}