using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.SpeciesManagement.Entities;
using PetFamily.Domain.VolunteerManagement.Entities;

namespace PetFamily.Infrastructure.DbContexts;

// add-migration -context WriteDbContext Init
public class WriteDbContext(IConfiguration configuration) : DbContext
{

	public DbSet<Volunteer> Volunteers => Set<Volunteer>();

	public DbSet<Species> Species => Set<Species>();


	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseNpgsql(configuration.GetConnectionString(Constants.DATABASE))
			.UseSnakeCaseNamingConvention();

		optionsBuilder.UseSnakeCaseNamingConvention();

		optionsBuilder.EnableSensitiveDataLogging();

		optionsBuilder.UseLoggerFactory(CreateLoggerFactory());

		//optionsBuilder.AddInterceptors(new SoftDeleteInterceptor());
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(
			typeof(WriteDbContext).Assembly, 
			t => t.FullName?.Contains("Configurations.Write") ?? false);
	}

	ILoggerFactory CreateLoggerFactory() => 
		LoggerFactory.Create(b => b.AddConsole());
}
