namespace PetFamily.Core.DTOs;

public class BreedDto
{
	public Guid Id { get; init; }

	public Guid SpeciesId { get; init; }

	public string Name { get; set; } = string.Empty;
}