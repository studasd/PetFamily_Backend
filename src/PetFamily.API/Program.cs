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
builder.Services.AddSwaggerExamplesFromAssemblyOf<VolunteerRequestExample>(); // ����������� ��������


// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration)
	.AddContracts();

//builder.Services.AddFluentValidationAutoValidation(config =>
//{
//	config.OverrideDefaultResultFactoryWith<CustomResultFactory>();
//});


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
