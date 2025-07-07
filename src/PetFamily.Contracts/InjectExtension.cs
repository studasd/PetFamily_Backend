using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Contracts.Volonteers.Create;

namespace PetFamily.Contracts;

public static class InjectExtension
{
	public static IServiceCollection AddContracts(this IServiceCollection services)
	{
		services.AddScoped<CreateVolunteerHandler>();

		services.AddValidatorsFromAssembly(typeof(InjectExtension).Assembly);

		return services;
	}
}
