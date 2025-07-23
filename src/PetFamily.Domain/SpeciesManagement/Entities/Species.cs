using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Errores;
using PetFamily.Domain.SpeciesManagement.IDs;

namespace PetFamily.Domain.SpeciesManagement.Entities;

public class Species : Entity<SpeciesId>
{
	Species(SpeciesId id) : base(id) { }

	public Species(SpeciesId id, string name, IEnumerable<Breed> breeds) : base(id)
	{
		Name = name;
		breeds = breeds.ToList();
	}

	public string Name { get; set; }

	public IReadOnlyList<Breed> Breeds => breeds;
	private List<Breed> breeds { get; set; } = [];


	public static Guid NewId() => Guid.NewGuid();

	public static Result<Species, Error> Create(string name, IEnumerable<Breed> breeds)
	{

		if (string.IsNullOrWhiteSpace(name))
			return Errors.General.ValueIsRequired("Name");

		var species = new Species(SpeciesId.NewSpeciesId(), name, breeds);

		return species;
	}


	public UnitResult<Error> AddBreeds(IEnumerable<Breed> breeds)
	{
		this.breeds.AddRange(breeds);

		return UnitResult.Success<Error>();
	}


	public UnitResult<Error> DeleteBreed(BreedId breedId)
	{
		var breed = breeds.FirstOrDefault(b => b.Id == breedId);
		if (breed is null)
			return Errors.General.NotFound(breedId.Value);

		var result = breeds.Remove(breed);

		return UnitResult.Success<Error>();
	}
}
