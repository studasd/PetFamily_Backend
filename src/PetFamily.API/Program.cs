using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
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
	c.ExampleFilters();
});
builder.Services.AddSwaggerExamplesFromAssemblyOf<VolunteerRequestExample>(); // Регистрация примеров


// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration)
	.AddContracts();

builder.Services
	.AddAuthentication("custom")
	.AddScheme<AuthOptions, CustomAuth>("custom", "custom", null);


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();

	await app.ApplyMigrationAsync();
}

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapControllers();


// Configure the HTTP request pipeline.


app.Run();

public partial class Program;


public class AuthOptions : AuthenticationSchemeOptions
{

}

public class CustomAuth : AuthenticationHandler<AuthOptions>
{
	public CustomAuth(
		IOptionsMonitor<AuthOptions> options, 
		ILoggerFactory logger, 
		UrlEncoder encoder, 
		ISystemClock clock) : base(options, logger, encoder, clock)
	{
	}

	public CustomAuth(
		IOptionsMonitor<AuthOptions> options,
		ILoggerFactory logger,
		UrlEncoder encoder
	) : base(options, logger, encoder)
	{
	}


	protected override Task<AuthenticateResult> HandleAuthenticateAsync()
	{
		return AuthenticateAsync();
	}
}