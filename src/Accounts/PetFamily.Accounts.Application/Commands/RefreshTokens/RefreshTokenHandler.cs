using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Application.Abstractions;
using PetFamily.Accounts.Contracts.Responses;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Commands.RefreshTokens;

public class RefreshTokenHandler : ICommandHandler<LoginResponse, RefreshTokenCommand>
{
	private readonly UserManager<User> userManager;
	private readonly ITokenProvider tokenProvider;
	private readonly IRefreshSessionManager refreshSessionManager;
	private readonly IUnitOfWork unitOfWork;
	private readonly ILogger<RefreshTokenHandler> logger;

	public RefreshTokenHandler(
		UserManager<User> userManager,
		ITokenProvider tokenProvider,
		IRefreshSessionManager refreshSessionManager,
		IUnitOfWork unitOfWork,
		ILogger<RefreshTokenHandler> logger
		)
	{
		this.userManager = userManager;
		this.tokenProvider = tokenProvider;
		this.refreshSessionManager = refreshSessionManager;
		this.unitOfWork = unitOfWork;
		this.logger = logger;
	}


	public async Task<Result<LoginResponse, ErrorList>> HandleAsync(RefreshTokenCommand command, CancellationToken token)
	{
		var oldRefreshSessionResult = await refreshSessionManager.GetByRefreshTokenAsync(command.RefreshToken, token);
		if(oldRefreshSessionResult.IsFailure)
			return oldRefreshSessionResult.Error.ToErrorList();

		if(oldRefreshSessionResult.Value.ExpiresIn < DateTime.UtcNow)
			return Errors.Tokens.ExpiredToken().ToErrorList();


		var userClaims = await tokenProvider.GetUserClaimsAsync(command.AccessToken, token);
		if(userClaims.IsFailure)
			return userClaims.Error.ToErrorList();

		var userIdstring = userClaims.Value
			.FirstOrDefault(x => x.Type == CustomClaims.Id)?.Value;
		if (Guid.TryParse(userIdstring, out var userId))
			return Errors.General.Failure().ToErrorList();

		if(oldRefreshSessionResult.Value.UserId != userId)
			return Errors.Tokens.InvalidToken().ToErrorList();


		var userJtiString = userClaims.Value
			.FirstOrDefault(x => x.Type == CustomClaims.Jti)?.Value;
		if (Guid.TryParse(userJtiString, out var userJti))
			return Errors.General.Failure().ToErrorList();

		if (oldRefreshSessionResult.Value.Jti != userJti)
			return Errors.Tokens.InvalidToken().ToErrorList();

		refreshSessionManager.Delete(oldRefreshSessionResult.Value);
		await unitOfWork.SaveChangesAsync(token);


		var accessToken = await tokenProvider
			.GenerateAccessTokenAsync(oldRefreshSessionResult.Value.User, token);
		var refreshToken = await tokenProvider
			.GenerateRefreshTokenAsync(oldRefreshSessionResult.Value.User, accessToken.Jti, token);


		return new LoginResponse(accessToken.AccessToken, refreshToken);
	}
}
