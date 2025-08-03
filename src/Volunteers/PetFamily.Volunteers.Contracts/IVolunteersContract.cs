using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Contracts;

public interface IVolunteersContract
{
	Task<bool> IsAnySpeciesFromPetAsync(Guid SpeciesId, CancellationToken token);
	Task<bool> IsAnyBreedFromPetAsync(Guid BreedId, CancellationToken token);
}
