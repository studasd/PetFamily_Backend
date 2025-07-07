using Microsoft.EntityFrameworkCore;
using PetFamily.Infrastructure;

public static class AppExtensions
{
	public static async Task<WebApplication> ApplyMigrationAsync(this WebApplication app)
	{
		await using var scope = app.Services.CreateAsyncScope();
		
		var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

		await db.Database.MigrateAsync();

		return app;
	}
}