using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.Specieses.Application.Queries.CheckSpeciesBreedExist;
using PetFamily.Specieses.Contracts;
using PetFamily.Specieses.Contracts.Requests;

namespace PetFamily.Specieses.Presentation;

public class SpeciesContract : ISpeciesContract
{
	private readonly CheckSpeciesBreedExistHandler checkSpeciesBreedExistHandler;

	public SpeciesContract(CheckSpeciesBreedExistHandler checkSpeciesBreedExistHandler)
	{
		this.checkSpeciesBreedExistHandler = checkSpeciesBreedExistHandler;
	}

	public async Task<Result<bool, ErrorList>> CheckSpeciesBreedExistAsync(CheckSpeciesBreedExistRequest request, CancellationToken token)
	{
		var query = new CheckSpeciesBreedExistQuery(request.SpeciesId, request.BreedId);

		return await checkSpeciesBreedExistHandler.HandleAsync(query, token);
	}
}
