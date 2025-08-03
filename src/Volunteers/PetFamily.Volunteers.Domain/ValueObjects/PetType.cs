namespace PetFamily.Volunteers.Domain.ValueObjects;

public record PetType
{
	private PetType() { }

	public PetType(Guid breedId, Guid speciesId)
	{
		SpeciesId = speciesId;
		BreedId = breedId;
	}

	public Guid SpeciesId { get; }
	public Guid BreedId { get; }
}