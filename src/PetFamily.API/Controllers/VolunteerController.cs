using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Contracts.Volonteers.Create;
using PetFamily.Contracts.Volonteers.Update;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteerController : ControllerBase
{
	[HttpPost]
	public async Task<IActionResult> Create(
		[FromServices] CreateVolunteerHandler handler,
		[FromBody] CreateVolunteerRequest request,
		CancellationToken token = default)
	{
		var result = await handler.HandleAsync(request, token);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}


	[HttpPut("info/{id:guid}")]
	public async Task<IActionResult> Create(
		[FromQuery] Guid id,
		[FromServices] UpdateInfoHandler handler,
		[FromServices] IValidator<UpdateInfoRequest> validator,
		CancellationToken token = default)
	{
		var request = new UpdateInfoRequest(id);

		var validResult = await validator.ValidateAsync(request, token);

		if (validResult.IsValid == false)
		{
			return validResult.ToValidationErrorResponse();
		}

		var result = await handler.HandleAsync(request, token);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}
}
