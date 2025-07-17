using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Responses;
using PetFamily.Application.PetsManagement.Queries.GetPetsWithPagination;
using PetFamily.Contracts.RequestPets;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("pet")]
public class PetController : ApplicationController
{
	[HttpGet]
	public async Task<IActionResult> GetAll(
		[FromQuery] GetFilteredPetsWithPaginationRequest request,
		[FromServices] GetFilteredPetsWithPaginationHandler handler,
		CancellationToken token)
	{
		var query = new GetFilteredPetsWithPaginationQuery(request.Name, request.Page, request.PageSize);

		var response = await handler.HandleAsync(query, token);

		return Ok(response);
	}
}
