using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PetFamily.Domain.VolunteerEntities;

namespace PetFamily.Infrastructure;

public class ApplicationDbContext : DbContext
{
	public DbSet<Volunteer> Volunteers => Set<Volunteer>();

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseNpgsql();
	}
}
