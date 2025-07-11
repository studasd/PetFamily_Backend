using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.SpeciesManagement.Entities;
using PetFamily.Domain.SpeciesManagement.IDs;

namespace PetFamily.Application.Volonteers;

public interface ISpeciesRepository
{
	Task<Guid> AddAsync(Species species, CancellationToken token = default);
	Task<Guid> DeleteAsync(Species species, CancellationToken token = default);
	Task<Result<Species, Error>> GetByIdAsync(SpeciesId speciesId, CancellationToken token = default);
	Task<Result<Species, Error>> GetByNameAsync(string speciesName, CancellationToken token = default);
	Task SaveAsync(CancellationToken token = default);
}