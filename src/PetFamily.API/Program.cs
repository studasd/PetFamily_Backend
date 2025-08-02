using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PetFamily.Accounts.Infrastructure;
using PetFamily.API.Authorization;
using PetFamily.API.Examples;
using PetFamily.API.Middlewares;
using PetFamily.API.Validations;
using PetFamily.Application;
using PetFamily.Contracts;
using PetFamily.Infrastructure;
using Serilog;
using Serilog.Events;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;

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
builder.Services.AddInfrastructure(builder.Configuration)
	.AddInfrastructureAuthorization(builder.Configuration)
	.AddContracts();

builder.Services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();

	await app.ApplyMigrationAsync();
}

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


// Configure the HTTP request pipeline.


app.Run();

public partial class Program;
