using CSharpFunctionalExtensions;
using PetFamily.Accounts.Application.Commands.Register;
using PetFamily.Accounts.Contracts;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.Accounts.Infrastructure.IdentityManagers;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Presentation;

public class AccountsContract : IAccountsContract
{
	private readonly RegisterUserHandler registerUserHandler;
	private readonly PermissionManager permissionManager;

	public AccountsContract(
		RegisterUserHandler registerUserHandler,
		PermissionManager permissionManager)
	{
		this.registerUserHandler = registerUserHandler;
		this.permissionManager = permissionManager;
	}


	public async Task<HashSet<string>> GetUserPermissionCodesAsync(Guid userId)
	{
		return await permissionManager.GetUserPermissionCodesAsync(userId);
	}

	public async Task<UnitResult<ErrorList>> RegisterUserAsync(RegisterUserRequest request, CancellationToken token)
	{
		var command = new RegisterUserCommand(request.Email, request.Password, request.UserName);

		return await registerUserHandler.HandleAsync(command, token);
	}
}