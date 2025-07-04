using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.PetEntities;

public class Species : Entity<Guid>
{
	Species() { }

	public Species(Guid id, string name) : base(id)
	{
		Name = name;
	}

	public string Name { get; set; }
	public IReadOnlyList<Breed> Breeds { get; set; } = [];


	public static Guid NewId() => Guid.NewGuid();

	public static Result<Species> Create(string name)
	{

		if (string.IsNullOrWhiteSpace(name))
			return Result.Failure<Species>("Name cannot be empty");

		var species = new Species(NewId(), name);

		return Result.Success(species);
	}
}
