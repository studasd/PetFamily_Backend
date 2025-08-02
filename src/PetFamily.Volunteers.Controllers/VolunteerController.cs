using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Examples;
using PetFamily.API.Extensions;
using PetFamily.API.Processors;
using PetFamily.Contracts.RequestPets;
using PetFamily.Contracts.RequestVolonteers;
using PetFamily.Volunteers.Application.PetsManagement.Commands.Add;
using PetFamily.Volunteers.Application.PetsManagement.Commands.Delete;
using PetFamily.Volunteers.Application.PetsManagement.Commands.DeletePhotos;
using PetFamily.Volunteers.Application.PetsManagement.Commands.MovePosition;
using PetFamily.Volunteers.Application.PetsManagement.Commands.UpdateInfo;
using PetFamily.Volunteers.Application.PetsManagement.Commands.UpdatePrimePhoto;
using PetFamily.Volunteers.Application.PetsManagement.Commands.UpdateStatus;
using PetFamily.Volunteers.Application.PetsManagement.Commands.UploadPhotos;
using PetFamily.Volunteers.Application.VolunteerManagement.Queries.GetVolunteerWithPagination;
using PetFamily.Volunteers.Application.VolunteerManagement.UseCases.Create;
using PetFamily.Volunteers.Application.VolunteerManagement.UseCases.Delete;
using PetFamily.Volunteers.Application.VolunteerManagement.UseCases.Updates.BankingDetails;
using PetFamily.Volunteers.Application.VolunteerManagement.UseCases.Updates.Info;
using PetFamily.Volunteers.Application.VolunteerManagement.UseCases.Updates.SocialNetworks;
using Swashbuckle.AspNetCore.Filters;

namespace PetFamily.Volunteers.Controllers;

[ApiController]
[Route("volunteer")]
public class VolunteerController : ControllerBase
{

	[HttpPost]
	[SwaggerRequestExample(typeof(CreateVolunteerRequest), typeof(VolunteerRequestExample))]
	public async Task<IActionResult> Create(
		[FromServices] CreateVolunteerHandler handler,
		[FromBody] CreateVolunteerRequest request,
		CancellationToken token)
	{
		var command = new CreateVolunteerCommand(
			Name: request.Name,
			Email: request.Email,
			Description: request.Description,
			ExperienceYears: request.ExperienceYears,
			Phone: request.Phone,
			BankingDetails: request.BankingDetails ?? [],
			SocialNetworks: request.SocialNetworks ?? []
			);

		var result = await handler.HandleAsync(command, token);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}


	[HttpPost("pet/{volunteerId:guid}")]
	[SwaggerRequestExample(typeof(AddPetRequest), typeof(PetRequestExample))]
	public async Task<IActionResult> AddPet(
		[FromRoute] Guid volunteerId,
		[FromBody] AddPetRequest request,
		[FromServices] AddPetHandler handler,
		CancellationToken token)
	{
		var command = new AddPetCommand(
			VolunteerId: volunteerId,
			Name: request.Name,
			Description: request.Description,
			BreedId: request.BreedId,
			SpeciesId: request.SpeciesId,
			Color: request.Color,
			Weight: request.Weight,
			Height: request.Height,
			Phone: request.Phone,
			HelpStatus: request.HelpStatus,
			Address: request.AddressDTO
			);

		var result = await handler.HandleAsync(command, token);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}


	[HttpPut("info/{volunteerId:guid}")]
	public async Task<IActionResult> Update(
		[FromRoute] Guid volunteerId,
		[FromBody] UpdateInfoRequest request,
		[FromServices] UpdateInfoHandler handler,
		CancellationToken token)
	{
		var command = new UpdateInfoCommand(volunteerId, request.Name, request.Email, request.Description);

		var result = await handler.HandleAsync(command, token);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}


	[HttpPut("social-networks/{volunteerId:guid}")]
	public async Task<IActionResult> UpdateSocials(
		[FromRoute] Guid volunteerId,
		[FromBody] UpdateSocialNetworksRequest request,
		[FromServices] UpdateSocialNetworksHandler handler,
		CancellationToken token)
	{
		var command = new UpdateSocialNetworksCommand(volunteerId, request.SocialNetworks);

		var result = await handler.HandleAsync(command, token);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}


	[HttpPut("bankig-details/{volunteerId:guid}")]
	public async Task<IActionResult> UpdateBanking(
		[FromRoute] Guid volunteerId,
		[FromBody] UpdateBankingDetailsRequest request,
		[FromServices] UpdateBankingDetailsHandler handler,
		CancellationToken token)
	{
		var command = new UpdateBankingDetailsCommand(volunteerId, request.BankingDetails);

		var result = await handler.HandleAsync(command, token);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}


	[HttpDelete("hard/{volunteerId:guid}")]
	public async Task<IActionResult> DeleteHard(
		[FromRoute] Guid volunteerId,
		[FromServices] DeleteVolunteerHandler handler,
		CancellationToken token)
	{
		var command = new DeleteVolunteerCommand(volunteerId, IsSoftDelete: false);

		var result = await handler.HandleAsync(command, token);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}


	[HttpDelete("soft/{volunteerId:guid}")]
	public async Task<IActionResult> DeleteSoft(
		[FromRoute] Guid volunteerId,
		[FromServices] DeleteVolunteerHandler handler,
		CancellationToken token)
	{
		var command = new DeleteVolunteerCommand(volunteerId, IsSoftDelete:true);

		var result = await handler.HandleAsync(command, token);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}



	[HttpDelete("pet/hard/{volunteerId:guid}/{petId:guid}")]
	public async Task<IActionResult> DeletePetHard(
		[FromRoute] Guid volunteerId,
		[FromRoute] Guid petId,
		[FromServices] DeletePetHandler handler,
		CancellationToken token)
	{
		var command = new DeletePetCommand(volunteerId, petId, IsSoftDelete: false);

		var result = await handler.HandleAsync(command, token);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}


	[HttpDelete("pet/soft/{volunteerId:guid}/{petId:guid}")]
	public async Task<IActionResult> DeletePetSoft(
		[FromRoute] Guid volunteerId,
		[FromRoute] Guid petId,
		[FromServices] DeletePetHandler handler,
		CancellationToken token)
	{
		var command = new DeletePetCommand(volunteerId, petId, IsSoftDelete: true);

		var result = await handler.HandleAsync(command, token);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}


	[HttpPost("pet/photos/{volunteerId:guid}/{petId:guid}")]
	public async Task<IActionResult> UploadPetPhotos(
		[FromRoute] Guid volunteerId,
		[FromRoute] Guid petId,
		[FromServices] UploadPhotosPetHandler handler,
		[FromForm] UploadPetPhotosRequest request,
		CancellationToken token)
	{
		await using var fileProcessor = new FormFileProcessor();
		var fileDtos = fileProcessor.Process(request.PhotosUpload);

		var command = new UploadPhotosPetCommand(volunteerId, petId, fileDtos);

		var result = await handler.HandleAsync(command, token);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}


	[HttpDelete("pet/photos/{volunteerId:guid}/{petId:guid}")]
	public async Task<IActionResult> DeletePetPhotos(
		[FromRoute] Guid volunteerId,
		[FromRoute] Guid petId,
		[FromServices] DeletePhotosPetHandler handler,
		[FromBody] DeletePetPhotosRequest request,
		CancellationToken token)
	{
		var command = new DeletePhotosPetCommand(volunteerId, petId, request.PhotosDelete);

		var result = await handler.HandleAsync(command, token);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}



	[HttpPut("pet/move-position/{volunteerId:guid}/{petId:guid}/{newPosition:int}")]
	public async Task<IActionResult> MovePetPosition(
		[FromRoute] Guid volunteerId,
		[FromRoute] Guid petId,
		[FromRoute] int newPosition,
		[FromServices] MovePositionPetHandler handler,
		CancellationToken token)
	{
		var command = new MovePositionPetCommand(volunteerId, petId, newPosition);

		var result = await handler.HandleAsync(command, token);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}


	[HttpPut("pet/{volunteerId:guid}/{petId:guid}")]
	public async Task<IActionResult> UpdatePetInfo(
		[FromRoute] Guid volunteerId,
		[FromRoute] Guid petId,
		[FromBody] UpdatePetInfoRequest request,
		[FromServices] UpdatePetInfoHandler handler,
		CancellationToken token)
	{
		var command = new UpdatePetInfoCommand(
			volunteerId, 
			petId, 
			request.SpeciesId, 
			request.BreedId, 
			request.Name, 
			request.Description, 
			request.Color, 
			request.HealthInfo, 
			request.Address, 
			request.DateBirth,
			request.Weight, 
			request.Height, 
			request.Phones, 
			request.IsNeutered, 
			request.IsVaccinated, 
			request.HelpStatus, 
			request.BankingВetails
			);

		var result = await handler.HandleAsync(command, token);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}


	[HttpPut("pet/status/{volunteerId:guid}/{petId:guid}")]
	public async Task<IActionResult> UpdatePetStatus(
		[FromRoute] Guid volunteerId,
		[FromRoute] Guid petId,
		[FromBody] UpdatePetStatusRequest request,
		[FromServices] UpdatePetStatusHandler handler,
		CancellationToken token)
	{
		var command = new UpdatePetStatusCommand(volunteerId, petId, request.HelpStatus);

		var result = await handler.HandleAsync(command, token);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}


	[HttpPut("pet/prime-photo/{volunteerId:guid}/{petId:guid}")]
	public async Task<IActionResult> UpdatePetPrimePhoto(
		[FromRoute] Guid volunteerId,
		[FromRoute] Guid petId,
		[FromBody] UpdatePetPrimePhotoRequest request,
		[FromServices] UpdatePetPrimePhotoHandler handler,
		CancellationToken token)
	{
		var command = new UpdatePetPrimePhotoCommand(volunteerId, petId, request.PathPhoto);

		var result = await handler.HandleAsync(command, token);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}


	[HttpGet("/{volunteerId:guid}")]
	public async Task<IActionResult> GetById(
		[FromRoute] Guid volunteerId,
		[FromQuery] GetVolunteerByIdRequest request,
		[FromServices] GetVolunteerByIdHandler handler,
		CancellationToken token)
	{
		var query = new GetVolunteerByIdQuery(
			volunteerId,
			request.Page,
			request.PageSize);

		var response = await handler.HandleAsync(query, token);

		return Ok(response);
	}
}
