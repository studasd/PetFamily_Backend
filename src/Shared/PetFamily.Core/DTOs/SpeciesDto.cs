namespace PetFamily.Core.DTOs;

public class SpeciesDto
{
	public Guid Id { get; init; }

	public string Name { get; set; } = string.Empty;

	public BreedDto[] Breeds { get; init; } = [];
}
