using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.DTOs;

namespace PetFamily.Infrastructure.DbContexts;
public class ReadDbContext(string connectionString) : DbContext, IReadDbContext
{
	public IQueryable<VolunteerDto> Volunteers => Set<VolunteerDto>();

	public IQueryable<PetDto> Pets => Set<PetDto>();
	

	public IQueryable<SpeciesDto> Species => Set<SpeciesDto>();

	public IQueryable<BreedDto> Breeds => Set<BreedDto>();


	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		//optionsBuilder.UseNpgsql(configuration.GetConnectionString(Constants.DATABASE))
		optionsBuilder.UseNpgsql(connectionString)
			.UseSnakeCaseNamingConvention();

		optionsBuilder.UseSnakeCaseNamingConvention();

		optionsBuilder.EnableSensitiveDataLogging();

		optionsBuilder.UseLoggerFactory(CreateLoggerFactory());

		optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(
			typeof(ReadDbContext).Assembly,
			t => t.FullName?.Contains("Configurations.Read") ?? false);
	}

	ILoggerFactory CreateLoggerFactory() => 
		LoggerFactory.Create(b => b.AddConsole());
}