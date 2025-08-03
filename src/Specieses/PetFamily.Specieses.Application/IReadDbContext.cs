using PetFamily.Core.DTOs;

namespace PetFamily.Specieses.Application;

public interface IReadDbContext
{
	IQueryable<SpeciesDto> Species { get; }
	IQueryable<BreedDto> Breeds { get; }
}
