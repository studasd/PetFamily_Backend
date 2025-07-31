using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Authorization;
using PetFamily.Application.Authorization.DataModels;
using PetFamily.Domain.Shared.Errores;

namespace PetFamily.Application.AccountManagement.Commands.Login;

public class LoginUserHandler : ICommandHandler<string, LoginUserCommand>
{
	private readonly UserManager<User> userManager;
	private readonly ITokenProvider tokenProvider;
	private readonly ILogger<LoginUserHandler> logger;

	public LoginUserHandler(
		UserManager<User> userManager,
		ITokenProvider tokenProvider,
		ILogger<LoginUserHandler> logger
		)
	{
		this.userManager = userManager;
		this.tokenProvider = tokenProvider;
		this.logger = logger;
	}


	public async Task<Result<string, ErrorList>> HandleAsync(LoginUserCommand command, CancellationToken token)
	{
		var user = await userManager.FindByEmailAsync(command.Email);

		if (user == null)
		{
			return Errors.General.NotFound().ToErrorList();
		}

		var passwordComfirmed = await userManager.CheckPasswordAsync(user, command.Password);
		if(passwordComfirmed == false)
			return Errors.User.InvalidCredentials().ToErrorList();

		var tokenJwt = tokenProvider.GenerateAccessToken(user);

		logger.LogInformation("Successfully logged in.");

		return tokenJwt;
	}
}
