using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Pets.Create;
using PetFamily.Application.Pets.DeletePhotos;
using PetFamily.Application.Pets.MovePosition;
using PetFamily.Application.Pets.UploadPhotos;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.Delete;
using PetFamily.Application.Volunteers.Updates.BankingDetails;
using PetFamily.Application.Volunteers.Updates.Info;
using PetFamily.Application.Volunteers.Updates.SocialNetworks;

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
