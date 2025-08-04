using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application;
using PetFamily.Accounts.Contracts;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Presentation;
using PetFamily.Core.Options;
using System.Text;

namespace PetFamily.Accounts.Infrastructure;

public static class InjectExtension
{
	public static IServiceCollection AddAccountsInfrastructure(this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddTransient<ITokenProvider, JwtTokenProvider>();

		services.AddScoped<IAccountsContract, AccountsContract>();

		services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.JWT));

		services.AddOptions<JwtOptions>();

		services.AddIdentity<User, Role>(options => options.User.RequireUniqueEmail = true )
			.AddEntityFrameworkStores<AuthorizationDbContext>()
			.AddDefaultTokenProviders();

		services.AddScoped<AuthorizationDbContext>();


		var jwtOptions = configuration.GetSection(JwtOptions.JWT).Get<JwtOptions>()
			?? throw new ApplicationException("Missing jwt configuration");


		services
			.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(option =>
			{
				option.TokenValidationParameters = new TokenValidationParameters
				{
					ValidIssuer = jwtOptions.Issuer,
					ValidAudience = jwtOptions.Audience,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ClockSkew = TimeSpan.Zero
				};
			});

		services.AddAuthorization();

		return services;
	}

}
