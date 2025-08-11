using CSharpFunctionalExtensions;
using PetFamily.Accounts.Application.Models;
using PetFamily.Accounts.Domain;
using PetFamily.SharedKernel;
using System.Security.Claims;

namespace PetFamily.Accounts.Application.Abstractions;

public interface ITokenProvider
{
	Task<JwtTokenResult> GenerateAccessTokenAsync(User user, CancellationToken token);
	Task<Guid> GenerateRefreshTokenAsync(User user, Guid jti, CancellationToken token);
	Task<Result<IReadOnlyList<Claim>, Error>> GetUserClaimsAsync(string jwtToken, CancellationToken token);
}
