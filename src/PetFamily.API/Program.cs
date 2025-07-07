using PetFamily.API.Validations;
using PetFamily.Contracts;
using PetFamily.Infrastructure;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.AddInfrastructure()
	.AddContracts();

builder.Services.AddFluentValidationAutoValidation(config =>
{
	config.OverrideDefaultResultFactoryWith<CustomResultFactory>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();

	await app.ApplyMigrationAsync();
}

app.UseAuthorization();

app.MapControllers();


// Configure the HTTP request pipeline.


app.Run();
