using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain;
using PetFamily.SharedKernel;
using System.Text.Json;

namespace PetFamily.Accounts.Infrastructure;

// add-migration -context AccountsDbContext Accounts_Init
// update-database -context AccountsDbContext
public class AccountsDbContext (IConfiguration configuration) : IdentityDbContext<User, Role, Guid>
{

	public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
	public DbSet<Permission> Permissions => Set<Permission>();


	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseNpgsql(configuration.GetConnectionString(Constants.DATABASE))
			.UseSnakeCaseNamingConvention();

		optionsBuilder.UseSnakeCaseNamingConvention();

		optionsBuilder.EnableSensitiveDataLogging();

		optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<User>()
			.ToTable("users");

		modelBuilder.Entity<User>()
			.Property(u => u.SocialNetworks)
			.HasConversion(
			u => JsonSerializer.Serialize(u, JsonSerializerOptions.Default),
			json => JsonSerializer.Deserialize<List<SocialNetwork>>(json, JsonSerializerOptions.Default)!);

		modelBuilder.Entity<Role>()
			.ToTable("roles");

		modelBuilder.Entity<Permission>()
			.ToTable("permissions");

		modelBuilder.Entity<Permission>()
			.HasIndex(p => p.Code)
			.IsUnique();

		modelBuilder.Entity<RolePermission>()
			.ToTable("role_permissions");

		modelBuilder.Entity<RolePermission>()
			.HasOne(x => x.Role)
			.WithMany(r => r.RolePermissions)
			.HasForeignKey(r => r.RoleId);

		modelBuilder.Entity<RolePermission>()
			.HasOne(x => x.Permission)
			.WithMany()
			.HasForeignKey(r => r.PermissionId);

		modelBuilder.Entity<RolePermission>()
			.HasKey(x => new { x.RoleId, x.PermissionId });

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
