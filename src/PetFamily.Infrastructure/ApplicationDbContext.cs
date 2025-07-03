using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.VolunteerEntities;

namespace PetFamily.Infrastructure;

public class ApplicationDbContext(IConfiguration configuration) : DbContext
{
	private const string DATABASE = "Database";


	public DbSet<Volunteer> Volunteers => Set<Volunteer>();


	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseNpgsql(configuration.GetConnectionString(DATABASE));

		optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
	}

	ILoggerFactory CreateLoggerFactory() => 
		LoggerFactory.Create(b => b.AddConsole());
}
