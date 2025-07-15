using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Errores;
using PetFamily.Domain.SpeciesManagement.IDs;

namespace PetFamily.Domain.VolunteerManagement.ValueObjects;

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