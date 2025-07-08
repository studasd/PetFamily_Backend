using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.VolunteerManagement.Entities;
using PetFamily.Infrastructure.Interceptors;

namespace PetFamily.Infrastructure;

public class ApplicationDbContext(IConfiguration configuration) : DbContext
{
	private const string DATABASE = "Database";

	
	public DbSet<Volunteer> Volunteers => Set<Volunteer>();


	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseNpgsql(configuration.GetConnectionString(DATABASE))
			.UseSnakeCaseNamingConvention();

		optionsBuilder.UseSnakeCaseNamingConvention();

		optionsBuilder.EnableSensitiveDataLogging();

		optionsBuilder.UseLoggerFactory(CreateLoggerFactory());

		optionsBuilder.AddInterceptors(new SoftDeleteInterceptor());
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
	}

	ILoggerFactory CreateLoggerFactory() => 
		LoggerFactory.Create(b => b.AddConsole());
}
