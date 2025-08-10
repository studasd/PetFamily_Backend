using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application.Abstractions;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Models;
using PetFamily.Core.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PetFamily.Accounts.Infrastructure;

public class JwtTokenProvider : ITokenProvider
{
	private readonly JwtOptions jwtOptions;

	public JwtTokenProvider(IOptions<JwtOptions> options)
	{
		jwtOptions = options.Value;
	}


	public string GenerateAccessToken(User user)
	{
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var roleClaims = user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name!));

		Claim[] claims = new[]
		{
			new Claim(CustomClaims.Id, user.Id.ToString()),
			new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
		};
		claims = claims.Concat(roleClaims).ToArray();

		var tokenJwt = new JwtSecurityToken(
			issuer: jwtOptions.Issuer,
			audience: jwtOptions.Audience,
			expires: DateTime.UtcNow.AddMinutes(jwtOptions.ExpiredMinutesTime),
			claims: claims,
			signingCredentials: creds
		);

		var stringToken = new JwtSecurityTokenHandler().WriteToken(tokenJwt);

		return stringToken;
	}
}
