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
		[FromQuery] GetPetsWithPaginationRequest request,
		[FromServices] GetPetsWithPaginationHandler handler,
		CancellationToken token)
	{
		var query = new GetPetsWithPaginationQuery(request.Page, request.PageSize);

		var response = await handler.HandleAsync(query, token);

		return Ok(response);
	}
}
