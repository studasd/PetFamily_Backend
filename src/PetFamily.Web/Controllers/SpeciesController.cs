using Microsoft.AspNetCore.Mvc;
using PetFamily.Contracts.RequestBreeds;
using PetFamily.Core;
using PetFamily.Core.Extensions;
using PetFamily.Volunteers.Application.SpeciesManagemets.Commands.Delete;
using PetFamily.Volunteers.Application.SpeciesManagemets.Queries.GetBreedsPagination;
using PetFamily.Volunteers.Application.SpeciesManagemets.Queries.GetSpeciesPagination;

namespace PetFamily.Web.Controllers;

[ApiController]
[Route("species")]
public class SpeciesController : ApplicationController
{

	[HttpDelete("{speciesId:guid}")]
	public async Task<IActionResult> DeleteSpecies(
		[FromRoute] Guid speciesId,
		[FromServices] DeleteSpeciesHandler handler,
		CancellationToken cancellationToken)
	{
		var command = new DeleteSpeciesCommand(speciesId);
		var result = await handler.HandleAsync(command, cancellationToken);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}


	[HttpDelete("breed/{speciesId:guid}/{breedId:guid}")]
	public async Task<IActionResult> DeleteBreed(
		[FromRoute] Guid speciesId,
		[FromRoute] Guid breedId,
		[FromServices] DeleteBreedHandler handler,
		CancellationToken cancellationToken)
	{
		var command = new DeleteBreedCommand(speciesId, breedId);
		var result = await handler.HandleAsync(command, cancellationToken);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}


	[HttpGet]
	public async Task<ActionResult> GetAllSpecies(
		[FromQuery] GetFilteredSpeciesWithPaginationRequest request,
		[FromServices] GetFilteredSpeciesWithPaginationHandler handler,
		CancellationToken token)
	{
		var query = new GetFilteredSpeciesWithPaginationQuery(request.Page, request.PageSize);

		var response = await handler.HandleAsync(query, token);

		return Ok(response);
	}


	[HttpGet("breeds/{speciesId:guid}")]
	public async Task<ActionResult> GetAllBreeds(
		[FromRoute] Guid speciesId,
		[FromQuery] GetFilteredBreedsWithPaginationRequest request,
		[FromServices] GetFilteredBreedsWithPaginationHandler handler,
		CancellationToken token)
	{
		var query = new GetFilteredBreedsWithPaginationQuery(speciesId, request.Page, request.PageSize);

		var response = await handler.HandleAsync(query, token);

		return Ok(response);
	}
}
