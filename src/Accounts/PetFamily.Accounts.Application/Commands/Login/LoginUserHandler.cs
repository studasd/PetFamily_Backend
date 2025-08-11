using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Application.Abstractions;
using PetFamily.Accounts.Contracts.Responses;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Commands.Login;

public class LoginUserHandler : ICommandHandler<LoginResponse, LoginUserCommand>
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


	public async Task<Result<LoginResponse, ErrorList>> HandleAsync(LoginUserCommand command, CancellationToken token)
	{
		var user = await userManager.FindByEmailAsync(command.Email);

		if (user == null)
		{
			return Errors.General.NotFound().ToErrorList();
		}

		var passwordComfirmed = await userManager.CheckPasswordAsync(user, command.Password);
		if(passwordComfirmed == false)
			return Errors.User.InvalidCredentials().ToErrorList();

		var accessToken = await tokenProvider.GenerateAccessTokenAsync(user, token);
		var refreshToken = await tokenProvider.GenerateRefreshTokenAsync(user, accessToken.Jti, token);

		logger.LogInformation("Successfully logged in.");

		return new LoginResponse(accessToken.AccessToken, refreshToken);
	}
}
