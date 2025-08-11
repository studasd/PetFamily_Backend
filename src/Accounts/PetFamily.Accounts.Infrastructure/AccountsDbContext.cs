using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using System.Text.Json;

namespace PetFamily.Accounts.Infrastructure;

// add-migration -context AccountsDbContext Accounts_Init
// update-database -context AccountsDbContext
public class AccountsDbContext (string connectionString) : IdentityDbContext<User, Role, Guid>
{

	public DbSet<Permission> Permissions => Set<Permission>();

	public DbSet<RolePermission> RolePermissions => Set<RolePermission>();


	public DbSet<AdminAccount> AdminAccounts => Set<AdminAccount>();

	public DbSet<VolunteerAccount> VolunteerAccounts => Set<VolunteerAccount>();

	public DbSet<ParticipantAccount> ParticipantAccounts => Set<ParticipantAccount>();
	
	public DbSet<RefreshSession> RefreshSessions => Set<RefreshSession>();


	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseNpgsql(connectionString)
			.UseSnakeCaseNamingConvention();

		optionsBuilder.UseSnakeCaseNamingConvention();

		optionsBuilder.EnableSensitiveDataLogging();

		optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		
		modelBuilder.Entity<Role>()
			.ToTable("roles");
		
		
		modelBuilder.Entity<RefreshSession>()
			.ToTable("refresh_session");

		modelBuilder.Entity<RefreshSession>()
			.HasOne(r => r.User)
			.WithMany()
			.HasForeignKey(r => r.UserId);


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


		modelBuilder.ApplyConfigurationsFromAssembly(
			typeof(AccountsDbContext).Assembly,
			t => t.FullName?.Contains("Configurations") ?? false);
	}


	ILoggerFactory CreateLoggerFactory() =>
		LoggerFactory.Create(b => b.AddConsole());
}
