using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Application.Volonteers;
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
		services.AddScoped<ISpeciesRepository, SpeciesRepository>();
		
		services.AddScoped<DeleteExpiredVolunteerService>();

		services.AddMinio(opt =>
		{
			opt.WithEndpoint("http://localhost:9000");

			opt.WithCredentials("minioadmin", "minioadmin");

			opt.WithSSL(false);
		});

		return services;
	}
}
