using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.AccountManagement.Commands;
using PetFamily.Application.AccountManagement.Commands.Register;
using PetFamily.Contracts.RequestAccounts;

namespace PetFamily.API.Controllers.Accounts;
public class AccountController : ApplicationController
{

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
}
