using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PetFamily.Infrastructure.Authentication;

public static class InjectExtension
{
	public static IServiceCollection AddInfrastructureAuthorization(this IServiceCollection services)
	{
		services.AddIdentity<User, Role>()
			.AddEntityFrameworkStores<AuthorizationDbContext>();

		services.AddScoped<AuthorizationDbContext>();

		services
			.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(option =>
			{
				option.TokenValidationParameters = new TokenValidationParameters
				{
					ValidIssuer = "test",
					ValidAudience = "test",
					IssuerSigningKey = new SymmetricSecurityKey(
						Encoding.UTF8.GetBytes("tjtyjtyuj56ujy5rttytijkyjkytujhrtjh45rth455")),
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = false,
					ValidateIssuerSigningKey = true
				};
			});

		services.AddAuthorization();

		return services;
	}

}
