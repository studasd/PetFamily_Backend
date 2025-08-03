using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Specieses.Domain.Entities;

namespace PetFamily.Specieses.Infrastructure.DbContexts;

// add-migration -context WriteDbContext Init
// update-database -context WriteDbContext
public class WriteDbContext(string connectionString) : DbContext
{
	public DbSet<Species> Species => Set<Species>();


	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		//optionsBuilder.UseNpgsql(configuration.GetConnectionString(Constants.DATABASE))
		optionsBuilder.UseNpgsql(connectionString);

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
