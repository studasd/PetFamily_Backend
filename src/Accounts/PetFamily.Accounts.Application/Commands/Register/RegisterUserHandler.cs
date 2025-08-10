using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Application.Abstractions;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Commands.Register;

public class RegisterUserHandler : ICommandHandler<RegisterUserCommand>
{
	private readonly UserManager<User> userManager;
	private readonly RoleManager<Role> roleManager;
private readonly IAccountsManager accountsManager;
	private readonly ILogger<RegisterUserHandler> logger;

	public RegisterUserHandler(
		UserManager<User> userManager,
		RoleManager<Role> roleManager,
		IAccountsManager accountsManager,
		ILogger<RegisterUserHandler> logger
		)
	{
		this.userManager = userManager;
		this.roleManager = roleManager;
		this.accountsManager = accountsManager;
		this.logger = logger;
	}


	public async Task<UnitResult<ErrorList>> HandleAsync(RegisterUserCommand command, CancellationToken token)
	{
		var existedUser = await userManager.FindByEmailAsync(command.Email);
		if(existedUser != null)
			return Errors.General.AlreadyExist("Email").ToErrorList();

		var role = await roleManager.FindByNameAsync(ParticipantAccount.PARTICIPANT);
		if (role == null)
			return Errors.General.ValueIsInvalid("Role").ToErrorList();

		var userResult = User.CreateParticipant(command.UserName, command.Email, role);
		if (userResult.IsFailure)
			return userResult.Error.ToErrorList();

		var result = await userManager.CreateAsync(userResult.Value, command.Password);
		if (result.Succeeded == false)
		{
			var resultErrors = result.Errors.Select(e => Error.Failure(e.Code, e.Description));
			logger.LogInformation("Failed to create user from username: {Username}", command.UserName);
			return new ErrorList(resultErrors);
		}

		var participantAccount = new ParticipantAccount(userResult.Value);
		await accountsManager.CreateParticipantAccountAsync(participantAccount, token);

		logger.LogInformation("User created: {userName} a new account with password", command.UserName);

		return Result.Success<ErrorList>();
	}
}
