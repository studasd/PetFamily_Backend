using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Pets.Create;
using PetFamily.Application.Pets.DeletePhotos;
using PetFamily.Application.Pets.MovePosition;
using PetFamily.Application.Pets.UploadPhotos;
using PetFamily.Application.Volonteers.Create;
using PetFamily.Application.Volonteers.Delete;
using PetFamily.Application.Volonteers.Updates.BankingDetails;
using PetFamily.Application.Volonteers.Updates.Info;
using PetFamily.Application.Volonteers.Updates.SocialNetworks;

namespace PetFamily.Application;

public static class InjectExtension
{
	public static IServiceCollection AddContracts(this IServiceCollection services)
	{
		services.AddScoped<CreateVolunteerHandler>();
		services.AddScoped<AddPetHandler>();
		services.AddScoped<DeleteVolunteerHandler>();
		services.AddScoped<DeletePhotosPetHandler>();
		services.AddScoped<UploadPhotosPetHandler>();
		services.AddScoped<MovePositionPetHandler>();
		services.AddScoped<UpdateInfoHandler>();
		services.AddScoped<UpdateSocialNetworksHandler>();
		services.AddScoped<UpdateBankingDetailsHandler>();

		services.AddValidatorsFromAssembly(typeof(InjectExtension).Assembly);

		return services;
	}
}
