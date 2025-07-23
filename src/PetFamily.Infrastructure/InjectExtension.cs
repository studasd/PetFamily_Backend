using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Application.Database;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Messaging;
using PetFamily.Application.SpeciesManagemets;
using PetFamily.Application.VolunteerManagement;
using PetFamily.Contracts.RequestVolonteers;
using PetFamily.Infrastructure.BackgroundServices;
using PetFamily.Infrastructure.DbContexts;
using PetFamily.Infrastructure.MessageQueues;
using PetFamily.Infrastructure.Options;
using PetFamily.Infrastructure.Providers;
using PetFamily.Infrastructure.Repositories;
using PetFamily.Infrastructure.Services;

namespace PetFamily.Infrastructure;

public static class InjectExtension
{
	public static IServiceCollection AddInfrastructure (this IServiceCollection services, IConfiguration config)
	{
		services.AddMinio(config);


		services.AddHostedService<DeleteExpiredVolunteerBackgroundService>();
		services.AddHostedService<FilesCleanerBackgroundService>();

		services.AddScoped<WriteDbContext>();
		services.AddScoped<IReadDbContext, ReadDbContext>();
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddSingleton<ISqlConnectFactory, SqlConnectFactory>();

		Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

		services.AddScoped<IVolunteerRepository, VolunteerRepository>();
		services.AddScoped<ISpeciesRepository, SpeciesRepository>();
		services.AddScoped<IFileProvider, MinioProvider>();

		services.AddScoped<DeleteExpiredVolunteerService>();

		services.AddSingleton<IMessageQueue<IEnumerable<FileInform>>, InMemoryMessageQueue<IEnumerable<FileInform>>>();


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
