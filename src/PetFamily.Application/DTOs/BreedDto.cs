namespace PetFamily.Application.DTOs;

public class BreedDto
{
	public Guid Id { get; init; }

	public Guid SpeciesId { get; init; }

	public string Name { get; set; } = string.Empty;
}