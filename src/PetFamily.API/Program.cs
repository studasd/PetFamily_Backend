using PetFamily.API.Middlewares;
using PetFamily.API.Validations;
using PetFamily.Contracts;
using PetFamily.Infrastructure;
using Serilog;
using Serilog.Events;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
	.WriteTo.Console()
	.MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
	.MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
	.MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
	.CreateLogger();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSerilog();

// Add services to the container.
builder.Services.AddInfrastructure()
	.AddContracts();

builder.Services.AddFluentValidationAutoValidation(config =>
{
	config.OverrideDefaultResultFactoryWith<CustomResultFactory>();
});


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
