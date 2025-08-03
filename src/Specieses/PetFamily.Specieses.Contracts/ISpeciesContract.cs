using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.Specieses.Contracts.Requests;

namespace PetFamily.Specieses.Contracts;

public interface ISpeciesContract
{
	Task<Result<bool, ErrorList>> CheckSpeciesBreedExistAsync(CheckSpeciesBreedExistRequest request, CancellationToken token);
}
