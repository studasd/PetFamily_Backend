using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Accounts.Application.AccountManagement.UseCases.Updates.SocialNetworks;
using PetFamily.Accounts.Application.Commands.Login;
using PetFamily.Accounts.Application.Commands.RefreshTokens;
using PetFamily.Accounts.Application.Commands.Register;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;

namespace PetFamily.Accounts.Presentation;

public class AccountController : ApplicationController
{
	[Permission(Permissions.Volunteer.VolunteerRead)]
	[HttpPost("admin")]
	public IActionResult TestAdmin()
	{
		return Ok();
	}
	
	[Authorize()]
	[HttpPost("user")]
	public IActionResult TestUser()
	{
		return Ok();
	}


	[HttpPost("registration")]
	public async Task<IActionResult> Register(
		[FromBody] RegisterUserRequest request,
		[FromServices] RegisterUserHandler handler,
		CancellationToken token
		)
	{
		var command = new RegisterUserCommand(request.Email, request.Password, request.UserName);

		var result = await handler.HandleAsync(command, token);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok();
	}


	[HttpPost("login")]
	public async Task<IActionResult> Login(
		[FromBody] LoginUserRequest request,
		[FromServices] LoginUserHandler handler,
		CancellationToken token
		)
	{
		var command = new LoginUserCommand(request.Email, request.Password);

		var result = await handler.HandleAsync(command, token);

		if (result.IsFailure)
			return result.Error.ToResponse();

		Response.Cookies.Append("RefreshToken", result.Value.RefreshToken.ToString());

		return Ok(result.Value);
	}


	[HttpPost("refresh")]
	public async Task<IActionResult> RefreshToken(
		[FromBody] RefreshTokenRequest request,
		[FromServices] RefreshTokenHandler handler,
		CancellationToken token
		)
	{
		var command = new RefreshTokenCommand(request.AccessToken, request.RefreshToken);

		var result = await handler.HandleAsync(command, token);

		if (result.IsFailure)
			return result.Error.ToResponse();

		Response.Cookies.Append("RefreshToken", result.Value.RefreshToken.ToString());

		return Ok(result.Value);
	}


	[HttpPut("social-networks/{userId:guid}")]
	public async Task<IActionResult> UpdateSocials(
		[FromRoute] Guid userId,
		[FromBody] UpdateSocialNetworksRequest request,
		[FromServices] UpdateSocialNetworksHandler handler,
		CancellationToken token)
	{
		var command = new UpdateSocialNetworksCommand(userId, request.SocialNetworks);

		var result = await handler.HandleAsync(command, token);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}
}
