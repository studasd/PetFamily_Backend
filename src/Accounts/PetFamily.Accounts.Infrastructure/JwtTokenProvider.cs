using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application.Abstractions;
using PetFamily.Accounts.Application.Models;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.IdentityManagers;
using PetFamily.Core.Models;
using PetFamily.Core.Options;
using PetFamily.Framework;
using PetFamily.SharedKernel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PetFamily.Accounts.Infrastructure;

public class JwtTokenProvider : ITokenProvider
{
	private readonly JwtOptions jwtOptions;
	private readonly RefreshSessionOptions refreshOptions;
	private readonly PermissionManager permissionManager;
	private readonly AccountsDbContext accountsDbContext;

	public JwtTokenProvider(
		IOptions<JwtOptions> optionsJwt, 
		IOptions<RefreshSessionOptions> optionsRefresh,
		PermissionManager permissionManager,
		AccountsDbContext accountsDbContext)
	{
		jwtOptions = optionsJwt.Value;
		refreshOptions = optionsRefresh.Value;
		this.permissionManager = permissionManager;
		this.accountsDbContext = accountsDbContext;
	}


	public async Task<JwtTokenResult> GenerateAccessTokenAsync(User user, CancellationToken token)
	{
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var roleClaims = user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name!));

		var permissions = await permissionManager.GetUserPermissionCodesAsync(user.Id, token);
		var permissionClaims = permissions.Select(p => new Claim(CustomClaims.Permission, p));

		var jti = Guid.NewGuid();

		Claim[] claims = new[]
		{
			new Claim(CustomClaims.Id, user.Id.ToString()),
			new Claim(CustomClaims.Jti, jti.ToString()),
			//new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
		};
		claims = claims
			.Concat(roleClaims)
			.Concat(permissionClaims)
			.ToArray();

		var tokenJwt = new JwtSecurityToken(
			issuer: jwtOptions.Issuer,
			audience: jwtOptions.Audience,
			expires: DateTime.UtcNow.AddMinutes(jwtOptions.ExpiredMinutesTime),
			claims: claims,
			signingCredentials: creds
		);

		var stringToken = new JwtSecurityTokenHandler().WriteToken(tokenJwt);



		return new JwtTokenResult(stringToken, jti);
	}


	public async Task<Guid> GenerateRefreshTokenAsync(User user, Guid jti, CancellationToken token)
	{
		var refreshSession = new RefreshSession
		{
			UserId = user.Id,
			User = user,
			RefreshToken = Guid.NewGuid(),
			Jti = jti,
			ExpiresIn = DateTime.UtcNow.AddDays(refreshOptions.ExpiredDaysTime),
			CreatedAt = DateTime.UtcNow
		};

		accountsDbContext.RefreshSessions.Add(refreshSession);

		await accountsDbContext.SaveChangesAsync(token);

		return refreshSession.RefreshToken;
	}


	public async Task<Result<IReadOnlyList<Claim>, Error>> GetUserClaimsAsync(string jwtToken, CancellationToken token)
	{
		var jwtHandler = new JwtSecurityTokenHandler();

		var validationParameteres = TokenValidationParametersFactory.CreateWithoutLifeTime(jwtOptions);

		var validationResult = await jwtHandler.ValidateTokenAsync(jwtToken, validationParameteres);

		if(validationResult.IsValid == false)
			return Errors.Tokens.InvalidToken();

		return validationResult.ClaimsIdentity.Claims.ToList();
	}
}
