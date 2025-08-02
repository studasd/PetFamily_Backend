using CSharpFunctionalExtensions;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.Core.Errores;

namespace PetFamily.Accounts.Contracts;

public interface IAccountsContract
{
	Task<UnitResult<ErrorList>> RegisterUserAsync(RegisterUserRequest request, CancellationToken token);
}

