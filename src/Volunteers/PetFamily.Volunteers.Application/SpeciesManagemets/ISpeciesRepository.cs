using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.SpeciesManagement.Entities;
using PetFamily.Volunteers.Domain.SpeciesManagement.IDs;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.SpeciesManagemets;

public interface ISpeciesRepository
{
	Task<Guid> AddAsync(Species species, CancellationToken token);
	Task<Guid> DeleteAsync(Species species, CancellationToken token);
	Task<Result<Species, Error>> GetByIdAsync(SpeciesId speciesId, CancellationToken token);
	Task<Result<Species, Error>> GetByNameAsync(string speciesName, CancellationToken token);
	Task<Result<PetType, Error>> GetPetTypeByNamesAsync(string speciesName, string breedName, CancellationToken token);
	Task SaveAsync(CancellationToken token);
}