using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.AccountManagement.Commands;
using PetFamily.Application.AccountManagement.Commands.Login;
using PetFamily.Application.AccountManagement.Commands.Register;
using PetFamily.Contracts.RequestAccounts;

namespace PetFamily.API.Controllers.Accounts;
public class AccountController : ApplicationController
{
	[Authorize(Policy = "CreatePetRequirement")]
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

		return Ok(result.Value);
	}
}
