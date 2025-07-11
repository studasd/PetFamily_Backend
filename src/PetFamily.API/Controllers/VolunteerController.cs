using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Examples;
using PetFamily.API.Extensions;
using PetFamily.Application.Pets.Create;
using PetFamily.Application.Volonteers.Create;
using PetFamily.Application.Volonteers.Delete;
using PetFamily.Application.Volonteers.Updates.BankingDetails;
using PetFamily.Application.Volonteers.Updates.Info;
using PetFamily.Application.Volonteers.Updates.SocialNetworks;
using PetFamily.Contracts.Pets;
using PetFamily.Contracts.Volonteers;
using Swashbuckle.AspNetCore.Filters;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteerController : ControllerBase
{
	[HttpPost]
	[SwaggerRequestExample(typeof(CreateVolunteerRequest), typeof(VolunteerRequestExample))]
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

	[HttpPost("pet/{id:guid}")]
	[SwaggerRequestExample(typeof(CreatePetRequestDTO), typeof(PetRequestExample))]
	public async Task<IActionResult> AddPet(
		[FromRoute] Guid id,
		[FromBody] CreatePetRequestDTO dto,
		[FromServices] CreatePetHandler handler,
		[FromServices] IValidator<CreatePetRequest> validator,
		CancellationToken token = default)
	{
		var request = new CreatePetRequest(id, dto);
		var validResult = await validator.ValidateAsync(request, token);
		if (validResult.IsValid == false)
			return validResult.ToValidationErrorResponse();

		var result = await handler.HandleAsync(request, token);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}


	[HttpPut("info/{id:guid}")]
	public async Task<IActionResult> Update(
		[FromRoute] Guid id,
		[FromBody] UpdateInfoRequestDTO dto,
		[FromServices] UpdateInfoHandler handler,
		[FromServices] IValidator<UpdateInfoRequest> validator,
		CancellationToken token = default)
	{
		var request = new UpdateInfoRequest(id, dto);

		var validResult = await validator.ValidateAsync(request, token);

		if (validResult.IsValid == false)
			return validResult.ToValidationErrorResponse();

		var result = await handler.HandleAsync(request, token);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}


	[HttpPut("social-networks/{id:guid}")]
	public async Task<IActionResult> UpdateSocials(
		[FromRoute] Guid id,
		[FromBody] UpdateSocialNetworksRequestDTO dto,
		[FromServices] UpdateSocialNetworksHandler handler,
		[FromServices] IValidator<UpdateSocialNetworksRequest> validator,
		CancellationToken token = default)
	{
		var request = new UpdateSocialNetworksRequest(id, dto);

		var validResult = await validator.ValidateAsync(request, token);

		if (validResult.IsValid == false)
			return validResult.ToValidationErrorResponse();

		var result = await handler.HandleAsync(request, token);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}


	[HttpPut("bankig-details/{id:guid}")]
	public async Task<IActionResult> UpdateBanking(
		[FromRoute] Guid id,
		[FromBody] UpdateBankingDetailsRequestDTO dto,
		[FromServices] UpdateBankingDetailsHandler handler,
		[FromServices] IValidator<UpdateBankingDetailsRequest> validator,
		CancellationToken token = default)
	{
		var request = new UpdateBankingDetailsRequest(id, dto);

		var validResult = await validator.ValidateAsync(request, token);

		if (validResult.IsValid == false)
			return validResult.ToValidationErrorResponse();

		var result = await handler.HandleAsync(request, token);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}


	[HttpDelete("hard/{id:guid}")]
	public async Task<IActionResult> HardDelete(
		[FromRoute] Guid id,
		[FromServices] DeleteVolunteerHandler handler,
		[FromServices] IValidator<DeleteVolunteerRequest> validator,
		CancellationToken token = default)
	{
		var request = new DeleteVolunteerRequest(id, IsSoftDelete: false);

		var validResult = await validator.ValidateAsync(request, token);

		if (validResult.IsValid == false)
			return validResult.ToValidationErrorResponse();

		var result = await handler.HandleAsync(request, token);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}


	[HttpDelete("soft/{id:guid}")]
	public async Task<IActionResult> SoftDelete(
		[FromRoute] Guid id,
		[FromServices] DeleteVolunteerHandler handler,
		[FromServices] IValidator<DeleteVolunteerRequest> validator,
		CancellationToken token = default)
	{
		var request = new DeleteVolunteerRequest(id, IsSoftDelete:true);

		var validResult = await validator.ValidateAsync(request, token);

		if (validResult.IsValid == false)
			return validResult.ToValidationErrorResponse();

		var result = await handler.HandleAsync(request, token);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}
}
