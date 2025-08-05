using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PetFamily.Accounts.Infrastructure;
using PetFamily.Accounts.Presentation;
using PetFamily.Core;
using PetFamily.Framework.Authorization;
using PetFamily.Volunteers.Infrastructure;
using PetFamily.Volunteers.Infrastructure.DbContexts;
using PetFamily.Volunteers.Presentation;
using PetFamily.Web.Middlewares;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.Filters;
using System.Text.Json.Serialization;
using PetFamily.Accounts.Application;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Presentation.Examples;
using PetFamily.Specieses.Infrastructure;
using PetFamily.Specieses.Application;
using PetFamily.Accounts.Infrastructure.Seeding;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
	.WriteTo.Console()
	.WriteTo.Seq(builder.Configuration.GetValue<string>("HostSeq"))
	.MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
	.MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
	.MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
	.CreateLogger();


builder.Services.AddControllers()
	.AddJsonOptions(o =>
	{
		o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
	});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSerilog();

builder.Services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "My API",
		Version = "v1"
	});
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Please insert JWT with Bearer into field",
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey
	});
	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{ 
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new string[] {}
		}
	});
	c.ExampleFilters();
});
builder.Services.AddSwaggerExamplesFromAssemblyOf<VolunteerRequestExample>(); // Регистрация примеров


// Add services to the container.
builder.Services
	.AddAccountsApplication()
	.AddAccountsInfrastructure(builder.Configuration)
	.AddAccountsPresentation()

	.AddVolunteerApplication()
	.AddVolunteerInfrastructure(builder.Configuration)

	.AddSpeciesApplication()
	.AddSpeciesInfrastructure(builder.Configuration)

	.AddContracts();

builder.Services.AddControllers();
	//.AddApplicationPart(typeof(AccountController).Assembly)
	//.AddApplicationPart(typeof(VolunteerController).Assembly)


var app = builder.Build();

var accountsSeeder = app.Services.GetRequiredService<AccountsSeeder>();
await accountsSeeder.SeedAsync();


app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();

	
	//await app.ApplyMigrationAsync();
	await using var scope = app.Services.CreateAsyncScope();
	var db = scope.ServiceProvider.GetRequiredService<VolunteerWriteDbContext>();
	//await db.Database.MigrateAsync();
	//await PetFamily.Volunteers.Infrastructure.DbTestInitializer.InitializeAsync(db);
}

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


// Configure the HTTP request pipeline.


app.Run();

public partial class Program;
