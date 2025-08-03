using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.SpeciesManagement.IDs;

namespace PetFamily.Volunteers.Domain.ValueObjects;

public record PetType
{
	private PetType() { }

	private PetType(Guid breedId, Guid speciesId)
	{
		SpeciesId = speciesId;
		BreedId = breedId;
	}

	public Guid SpeciesId { get; }
	public Guid BreedId { get; }

	public static Result<PetType, Error> Create(BreedId breedId, SpeciesId speciesId)
	{
		return new PetType(breedId, speciesId);
	}
}