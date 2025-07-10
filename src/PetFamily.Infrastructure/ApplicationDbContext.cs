using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.SpeciesManagement.Entities;
using PetFamily.Domain.VolunteerManagement.Entities;

namespace PetFamily.Infrastructure;

public class ApplicationDbContext(IConfiguration configuration) : DbContext
{
	private const string DATABASE = "Database";

	
	public DbSet<Volunteer> Volunteers => Set<Volunteer>();

	public DbSet<Species> Species => Set<Species>();


	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseNpgsql(configuration.GetConnectionString(DATABASE))
			.UseSnakeCaseNamingConvention();

		optionsBuilder.UseSnakeCaseNamingConvention();

		optionsBuilder.EnableSensitiveDataLogging();

		optionsBuilder.UseLoggerFactory(CreateLoggerFactory());

		//optionsBuilder.AddInterceptors(new SoftDeleteInterceptor());
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
	}

	ILoggerFactory CreateLoggerFactory() => 
		LoggerFactory.Create(b => b.AddConsole());
}
