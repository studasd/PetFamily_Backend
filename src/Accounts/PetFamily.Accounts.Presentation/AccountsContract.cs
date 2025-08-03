using CSharpFunctionalExtensions;
using PetFamily.Accounts.Application.Commands.Register;
using PetFamily.Accounts.Contracts;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Presentation;

public class AccountsContract(RegisterUserHandler registerUserHandler) : IAccountsContract
{
	public async Task<UnitResult<ErrorList>> RegisterUserAsync(RegisterUserRequest request, CancellationToken token)
	{
		var command = new RegisterUserCommand(request.Email, request.Password, request.UserName);

		return await registerUserHandler.HandleAsync(command, token);
	}
}