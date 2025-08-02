using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Core;
using PetFamily.Core.FileProvider;
using PetFamily.Core.Messaging;
using PetFamily.Infrastructure;
using PetFamily.Infrastructure.MessageQueues;
using PetFamily.Infrastructure.Options;
using PetFamily.Infrastructure.Providers;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Application.SpeciesManagemets;
using PetFamily.Volunteers.Application.VolunteerManagement;
using PetFamily.Volunteers.Infrastructure;
using PetFamily.Volunteers.Infrastructure.BackgroundServices;
using PetFamily.Volunteers.Infrastructure.DbContexts;
using PetFamily.Volunteers.Infrastructure.Repositories;
using PetFamily.Volunteers.Infrastructure.Services;

namespace PetFamily.Volunteers.Infrastructure;

public static class InjectExtension
{
	public static IServiceCollection AddInfrastructure (this IServiceCollection services, IConfiguration config)
	{
		services.AddMinio(config);


		services.AddHostedService<DeleteExpiredVolunteerBackgroundService>();
		services.AddHostedService<FilesCleanerBackgroundService>();

		services.AddScoped<WriteDbContext>(_ => 
			new WriteDbContext(config.GetConnectionString(Constants.DATABASE)));
		services.AddScoped<IReadDbContext, ReadDbContext>(_ => 
			new ReadDbContext(config.GetConnectionString(Constants.DATABASE)));
		//services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddSingleton<ISqlConnectFactory, SqlConnectFactory>();

		Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

		services.AddScoped<IVolunteerRepository, VolunteerRepository>();
		services.AddScoped<ISpeciesRepository, SpeciesRepository>();
		services.AddScoped<IFileProvider, MinioProvider>();

		services.AddScoped<DeleteExpiredVolunteerService>();

		services.AddSingleton<IMessageQueue<IEnumerable<FileInform>>, InMemoryMessageQueue<IEnumerable<FileInform>>>();


		return services;
	}


	public static async Task<WebApplication> ApplyMigrationAsync(this WebApplication app)
	{
		await using var scope = app.Services.CreateAsyncScope();

		var db = scope.ServiceProvider.GetRequiredService<WriteDbContext>();

		await db.Database.MigrateAsync();

		await DbTestInitializer.InitializeAsync(db);

		return app;
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
