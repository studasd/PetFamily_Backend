using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Application.Authorization;
using PetFamily.Application.Authorization.DataModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PetFamily.Infrastructure.Authentication;

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

		var claims = new[]
		{
			new Claim(CustomClaims.Sub, user.Id.ToString()),
			new Claim(CustomClaims.Email, user.Email ?? "")
		};

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
