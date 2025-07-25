using Microsoft.EntityFrameworkCore;
using PetFamily.Infrastructure;
using PetFamily.Infrastructure.DbContexts;

public static class AppExtensions
{
	public static async Task<WebApplication> ApplyMigrationAsync(this WebApplication app)
	{
		await using var scope = app.Services.CreateAsyncScope();
		
		var db = scope.ServiceProvider.GetRequiredService<WriteDbContext>();

		await db.Database.MigrateAsync();

		await DbTestInitializer.InitializeAsync(db);

		return app;
	}
}