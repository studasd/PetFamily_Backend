using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Errores;
using PetFamily.Domain.SpeciesManagement.Entities;
using PetFamily.Domain.SpeciesManagement.IDs;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.Volunteers;

public interface ISpeciesRepository
{
	Task<Guid> AddAsync(Species species, CancellationToken token);
	Task<Guid> DeleteAsync(Species species, CancellationToken token);
	Task<Result<Species, Error>> GetByIdAsync(SpeciesId speciesId, CancellationToken token);
	Task<Result<Species, Error>> GetByNameAsync(string speciesName, CancellationToken token);
	Task<Result<PetType, Error>> GetPetTypeByNamesAsync(string speciesName, string breedName, CancellationToken token);
	Task SaveAsync(CancellationToken token);
}