using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Application.Abstractions;

public interface ITokenProvider
{
	string GenerateAccessToken(User user);
}
