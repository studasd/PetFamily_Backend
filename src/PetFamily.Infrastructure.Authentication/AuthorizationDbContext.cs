using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.Authentication;

// add-migration -context AuthorizationDbContext InitAuthorization
// update-database -context AuthorizationDbContext
public class AuthorizationDbContext (IConfiguration configuration) : IdentityDbContext<User, Role, Guid>
{

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseNpgsql(configuration.GetConnectionString(Constants.DATABASE));

		optionsBuilder.UseSnakeCaseNamingConvention();

		optionsBuilder.EnableSensitiveDataLogging();

		optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<User>()
			.ToTable("users");

		modelBuilder.Entity<Role>()
			.ToTable("roles");

		modelBuilder.Entity<IdentityUserClaim<Guid>>()
			.ToTable("user_claims");

		modelBuilder.Entity<IdentityUserToken<Guid>>()
			.ToTable("user_tokens");

		modelBuilder.Entity<IdentityUserLogin<Guid>>()
			.ToTable("user_logins");

		modelBuilder.Entity<IdentityRoleClaim<Guid>>()
			.ToTable("role_claims");

		modelBuilder.Entity<IdentityUserRole<Guid>>()
			.ToTable("user_roles");
	}

	ILoggerFactory CreateLoggerFactory() =>
		LoggerFactory.Create(b => b.AddConsole());
}
