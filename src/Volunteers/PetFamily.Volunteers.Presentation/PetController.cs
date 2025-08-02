using Microsoft.AspNetCore.Mvc;
using PetFamily.Contracts.RequestPets;
using PetFamily.Core;
using PetFamily.Core.Extensions;
using PetFamily.Volunteers.Application.PetsManagement.Queries.GetPetById;
using PetFamily.Volunteers.Application.PetsManagement.Queries.GetPetsWithPagination;

namespace PetFamily.Volunteers.Controllers;

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
		var query = new GetFilteredPetsWithPaginationQuery(
			request.Page, 
			request.PageSize,
			request.VolunteerIds,
			request.Name,
			request.Age,
			request.SpeciesId,
			request.BreedId,
			request.Color,
			request.Weight,
			request.Height,
			request.Country,
			request.City,
			request.HelpStatus,
			request.PositionFrom, 
			request.PositionTo,
			request.SortBy,
			request.SortDirection
		);

		var result = await handler.HandleAsync(query, token);

		return Ok(result);
	}

	[HttpGet("dapper")]
	public async Task<IActionResult> GetAllDapper(
		[FromQuery] GetFilteredPetsWithPaginationRequest request,
		[FromServices] GetFilteredPetsWithPaginationDapper handler,
		CancellationToken token)
	{
		var query = new GetFilteredPetsWithPaginationQuery(
			request.Page,
			request.PageSize,
			request.VolunteerIds,
			request.Name,
			request.Age,
			request.SpeciesId,
			request.BreedId,
			request.Color,
			request.Weight,
			request.Height,
			request.Country,
			request.City,
			request.HelpStatus,
			request.PositionFrom,
			request.PositionTo,
			request.SortBy,
			request.SortDirection
		);

		var response = await handler.HandleAsync(query, token);

		return Ok(response);
	}


	[HttpGet("{petId:guid}")]
	public async Task<IActionResult> GetPetById(
		[FromRoute] Guid petId,
		[FromServices] GetPetByIdHandler handler,
		CancellationToken token)
	{
		var query = new GetPetByIdQuery(petId);

		var result = await handler.HandleAsync(query, token);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}
}
