using Microsoft.Extensions.DependencyInjection;
using PetFamily.Contracts.Volonteers.CreateVolonteer;

namespace PetFamily.Contracts;

public static class InjectExtension
{
	public static IServiceCollection AddContracts(this IServiceCollection services)
	{
		services.AddScoped<CreateVolunteerHandler>();

		return services;
	}
}
