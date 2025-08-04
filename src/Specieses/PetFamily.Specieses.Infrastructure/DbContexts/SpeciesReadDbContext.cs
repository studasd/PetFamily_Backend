using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Core.DTOs;
using PetFamily.Specieses.Application;

namespace PetFamily.Specieses.Infrastructure.DbContexts;
public class SpeciesReadDbContext(string connectionString) : DbContext, IReadDbContext
{
	public IQueryable<SpeciesDto> Species => Set<SpeciesDto>();

	public IQueryable<BreedDto> Breeds => Set<BreedDto>();


	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		//optionsBuilder.UseNpgsql(configuration.GetConnectionString(Constants.DATABASE))
		optionsBuilder.UseNpgsql(connectionString);

		optionsBuilder.EnableSensitiveDataLogging();

		optionsBuilder.UseLoggerFactory(CreateLoggerFactory());

		optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(
			typeof(SpeciesReadDbContext).Assembly,
			t => t.FullName?.Contains("Configurations.Read") ?? false);
	}

	ILoggerFactory CreateLoggerFactory() => 
		LoggerFactory.Create(b => b.AddConsole());
}