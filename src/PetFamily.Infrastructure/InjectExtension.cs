using Microsoft.Extensions.DependencyInjection;
using PetFamily.Contracts.Volonteers;
using PetFamily.Infrastructure.BackgroundServices;
using PetFamily.Infrastructure.Repositories;
using PetFamily.Infrastructure.Services;

namespace PetFamily.Infrastructure;

public static class InjectExtension
{
	public static IServiceCollection AddInfrastructure (this IServiceCollection services)
	{
		services.AddHostedService<DeleteExpiredVolunteerBackgroundService>();

		services.AddScoped<ApplicationDbContext>();

		services.AddScoped<IVolunteerRepository, VolunteerRepository>();
		
		services.AddScoped<DeleteExpiredVolunteerService>();

		return services;
	}
}
