using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Contracts.Volonteers.CreateVolonteer;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteerController : ControllerBase
{

	[HttpPost]
	public async Task<IActionResult> Create(
		[FromServices] CreateVolunteerHandler handler,
		[FromServices] IValidator<CreateVolunteerRequest> validator,
		[FromBody] CreateVolunteerRequest request, 
		CancellationToken token = default)
	{
		var validationResult = await validator.ValidateAsync(request, token);

		if (!validationResult.IsValid)
		{
			return validationResult.ToValidationErrorResponse();
		}

		var result = await handler.HandleAsync(request, token);

		if(result.IsFailure)
			return StatusCode(result.Error.TypeCode, result.Error);

		return Ok(result.Value);
	}
}
