using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Application.Providers;
using PetFamily.Application.Volonteers;
using PetFamily.Contracts.Volonteers;
using PetFamily.Infrastructure.BackgroundServices;
using PetFamily.Infrastructure.Options;
using PetFamily.Infrastructure.Providers;
using PetFamily.Infrastructure.Repositories;
using PetFamily.Infrastructure.Services;

namespace PetFamily.Infrastructure;

public static class InjectExtension
{
	public static IServiceCollection AddInfrastructure (this IServiceCollection services, IConfiguration config)
	{
		services.AddHostedService<DeleteExpiredVolunteerBackgroundService>();

		services.AddScoped<ApplicationDbContext>();

		services.AddScoped<IVolunteerRepository, VolunteerRepository>();
		services.AddScoped<ISpeciesRepository, SpeciesRepository>();
		services.AddScoped<IFileProvider, MinioProvider>();

		services.AddScoped<DeleteExpiredVolunteerService>();

		services.AddMinio(config);

		return services;
	}


	private static IServiceCollection AddMinio(this IServiceCollection services, IConfiguration config)
	{
		var optMinio = config.GetSection(MinioOptions.MINIO);
		services.Configure<MinioOptions>(optMinio);

		services.AddMinio(opt =>
		{
			var minioOptions = optMinio.Get<MinioOptions>()
					?? throw new ApplicationException("Missing minio configuration");

			opt.WithEndpoint(minioOptions.Endpoint);

			opt.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);

			opt.WithSSL(minioOptions.IsSSL);
		});

		return services;
	}
}
