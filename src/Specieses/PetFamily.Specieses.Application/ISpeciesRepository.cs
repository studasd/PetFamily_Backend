using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.Specieses.Domain.Entities;
using PetFamily.Specieses.Domain.IDs;

namespace PetFamily.Specieses.Application;

public interface ISpeciesRepository
{
	Task<Guid> AddAsync(Species species, CancellationToken token);
	Task<Guid> DeleteAsync(Species species, CancellationToken token);
	Task<Result<Species, Error>> GetByIdAsync(SpeciesId speciesId, CancellationToken token);
	Task<Result<Species, Error>> GetByNameAsync(string speciesName, CancellationToken token);
	Task SaveAsync(CancellationToken token);
}