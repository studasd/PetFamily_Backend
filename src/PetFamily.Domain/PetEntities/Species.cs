using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

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

	public static Result<Species, Error> Create(string name)
	{

		if (string.IsNullOrWhiteSpace(name))
			return Errors.General.ValueIsRequired("Name");

		var species = new Species(NewId(), name);

		return species;
	}
}
