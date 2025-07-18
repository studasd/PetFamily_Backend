using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.SpeciesManagemets.Commands.Delete;

namespace PetFamily.API.Controllers;

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
}
