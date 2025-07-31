using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Application.Authorization;
using PetFamily.Application.Authorization.DataModels;
using System.Text;

namespace PetFamily.Infrastructure.Authentication;

public static class InjectExtension
{
	public static IServiceCollection AddInfrastructureAuthorization(this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddTransient<ITokenProvider, JwtTokenProvider>();

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
					ValidateIssuerSigningKey = true
				};
			});

		services.AddAuthorization(options =>
		{
			options.DefaultPolicy = new AuthorizationPolicyBuilder()
				.RequireClaim("Role", "User")
				.RequireAuthenticatedUser()
				.Build();

			options.AddPolicy("RequireAdministratorRole", policy =>
			{
				policy.RequireClaim("Role", "Admin");
				policy.RequireAuthenticatedUser();
			});
		});

		return services;
	}

}
