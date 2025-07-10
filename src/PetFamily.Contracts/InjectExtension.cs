using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Contracts.Volonteers.Create;
using PetFamily.Contracts.Volonteers.Delete;
using PetFamily.Contracts.Volonteers.Update;

namespace PetFamily.Contracts;

public static class InjectExtension
{
	public static IServiceCollection AddContracts(this IServiceCollection services)
	{
		services.AddScoped<CreateVolunteerHandler>();
		services.AddScoped<UpdateInfoHandler>();
		services.AddScoped<DeleteVolunteerHandler>();

		services.AddValidatorsFromAssembly(typeof(InjectExtension).Assembly);

		return services;
	}
}
