using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Application.DTOs;
using PetFamily.Domain.SpeciesManagement.Entities;
using PetFamily.Domain.VolunteerManagement.Entities;

namespace PetFamily.Infrastructure.DbContexts;

public class ReadDbContext(IConfiguration configuration) : DbContext
{
	public DbSet<VolunteerDto> Volunteers => Set<VolunteerDto>();

	public DbSet<PetDto> Pets => Set<PetDto>();


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
		modelBuilder.ApplyConfigurationsFromAssembly(
			typeof(WriteDbContext).Assembly,
			t => t.FullName?.Contains("Configurations.Read") ?? false);
	}

	ILoggerFactory CreateLoggerFactory() => 
		LoggerFactory.Create(b => b.AddConsole());
}