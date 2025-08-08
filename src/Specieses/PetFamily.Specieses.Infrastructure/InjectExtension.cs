using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Core.FileProvider;
using PetFamily.Core.Messaging;
using PetFamily.SharedKernel;
using PetFamily.Specieses.Application;
using PetFamily.Specieses.Contracts;
using PetFamily.Specieses.Infrastructure;
using PetFamily.Specieses.Infrastructure.DbContexts;
using PetFamily.Specieses.Infrastructure.Repositories;
using PetFamily.Specieses.Presentation;

namespace PetFamily.Specieses.Infrastructure;

public static class InjectExtension
{
	public static IServiceCollection AddSpeciesInfrastructure(this IServiceCollection services, IConfiguration config)
	{
		services.AddScoped(_ => 
			new SpeciesWriteDbContext(config.GetConnectionString(Constants.DATABASE)));
		services.AddScoped<IReadDbContext, SpeciesReadDbContext>(_ => 
			new SpeciesReadDbContext(config.GetConnectionString(Constants.DATABASE)));
		//services.AddScoped<IUnitOfWork, UnitOfWork>();

		services.AddScoped<ISpeciesContract, SpeciesContract>();

		Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

		services.AddScoped<ISpeciesRepository, SpeciesRepository>();

		return services;
	}


	//public static async Task<WebApplication> ApplyMigrationAsync(this WebApplication app)
	//{
	//	await using var scope = app.Services.CreateAsyncScope();

	//	var db = scope.ServiceProvider.GetRequiredService<WriteDbContext>();

	//	await db.Database.MigrateAsync();

	//	await DbTestInitializer.InitializeAsync(db);

	//	return app;
	//}
}
