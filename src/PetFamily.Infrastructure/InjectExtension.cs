using Microsoft.Extensions.DependencyInjection;
using PetFamily.Contracts.Volonteers;
using PetFamily.Infrastructure.Repositories;

namespace PetFamily.Infrastructure;

public static class InjectExtension
{
	public static IServiceCollection AddInfrastructure (this IServiceCollection services)
	{
		services.AddScoped<ApplicationDbContext>();

		services.AddScoped<IVolunteerRepository, VolunteerRepository>();

		return services;
	}
}
