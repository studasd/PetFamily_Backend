using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Authorization.DataModels;
using PetFamily.Domain.Shared.Errores;

namespace PetFamily.Application.AccountManagement.Commands.Register;

public class RegisterUserHandler : ICommandHandler<RegisterUserCommand>
{
	private readonly UserManager<User> userManager;
	private readonly ILogger<RegisterUserHandler> logger;

	public RegisterUserHandler(
		UserManager<User> userManager,
		ILogger<RegisterUserHandler> logger
		)
	{
		this.userManager = userManager;
		this.logger = logger;
	}


	public async Task<UnitResult<ErrorList>> HandleAsync(RegisterUserCommand command, CancellationToken token)
	{
		var existedUser = await userManager.FindByEmailAsync(command.Email);

		if(existedUser != null)
		{
			return Errors.General.AlreadyExist("Email").ToErrorList();
		}

		var user = new User
		{
			Email = command.Email,
			UserName = command.UserName
		};

		var result = await userManager.CreateAsync(user, command.Password);

		if (result.Succeeded == true)
		{
			logger.LogInformation("User created: {userName} a new account with password", command.UserName);
			return Result.Success<ErrorList>();
		}

		var errors = result.Errors.Select(e => Error.Failure(e.Code, e.Description)).ToList();

		return new ErrorList(errors);
	}
}
