using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application.Abstractions;
using PetFamily.Accounts.Application.AccountManagement;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.IdentityManagers;
using PetFamily.Accounts.Infrastructure.Options;
using PetFamily.Accounts.Infrastructure.Repositories;
using PetFamily.Accounts.Infrastructure.Seeding;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Enums;
using PetFamily.Core.Options;
using PetFamily.Framework;
using PetFamily.SharedKernel;
using System.Text;

namespace PetFamily.Accounts.Infrastructure;

public static class InjectExtension
{
	public static IServiceCollection AddAccountsInfrastructure(this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddTransient<ITokenProvider, JwtTokenProvider>();

		services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.JWT));
		services.Configure<RefreshSessionOptions>(configuration.GetSection(RefreshSessionOptions.REFRESHSESSION));
		services.Configure<AdminOptions>(configuration.GetSection(AdminOptions.ADMIN));

		services.AddOptions<JwtOptions>();
		services.AddOptions<RefreshSessionOptions>();

		services.AddIdentity<User, Role>(options => options.User.RequireUniqueEmail = true )
			.AddEntityFrameworkStores<AccountsDbContext>()
			.AddDefaultTokenProviders();

		services.AddScoped<AccountsDbContext>(_ =>
			new AccountsDbContext(configuration.GetConnectionString(Constants.DATABASE)));

		services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(UnitOfWorkNames.Accounts);

		services.AddSingleton<AccountsSeeder>();
		services.AddScoped<AccountsSeederService>();
		services.AddScoped<PermissionManager>();
		services.AddScoped<RolePermissionManager>();
		services.AddScoped<IAccountsManager, AccountsManager>();
		services.AddScoped<IRefreshSessionManager, RefreshSessionManager>();

		services.AddScoped<IAccountRepository, AccountRepository>();


		services
			.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(option =>
			{
				var jwtOptions = configuration.GetSection(JwtOptions.JWT).Get<JwtOptions>()
					?? throw new ApplicationException("Missing jwt configuration");

				option.TokenValidationParameters = TokenValidationParametersFactory.CreateWithLifeTime(jwtOptions);
			});


		services.AddAuthorization();

		return services;
	}
}
