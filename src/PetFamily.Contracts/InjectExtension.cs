using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Contracts.Volonteers.Create;
using PetFamily.Contracts.Volonteers.Updates.Info;
using PetFamily.Contracts.Volonteers.Updates.SocialNetworks;

namespace PetFamily.Contracts;

public static class InjectExtension
{
	public static IServiceCollection AddContracts(this IServiceCollection services)
	{
		services.AddScoped<CreateVolunteerHandler>();
		services.AddScoped<UpdateInfoHandler>();
		services.AddScoped<UpdateSocialNetworksHandler>();

		services.AddValidatorsFromAssembly(typeof(InjectExtension).Assembly);

		return services;
	}
}
