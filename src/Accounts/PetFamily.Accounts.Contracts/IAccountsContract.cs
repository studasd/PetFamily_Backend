using CSharpFunctionalExtensions;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Contracts;

public interface IAccountsContract
{
	Task<UnitResult<ErrorList>> RegisterUserAsync(RegisterUserRequest request, CancellationToken token);

	Task<HashSet<string>> GetUserPermissionCodesAsync(Guid userId, CancellationToken token);
}

