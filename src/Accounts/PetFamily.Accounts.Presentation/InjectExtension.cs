using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Contracts;

namespace PetFamily.Accounts.Presentation;

public static class InjectExtension
{
	public static IServiceCollection AddAccountsPresentation(this IServiceCollection services)
	{
		services.AddScoped<IAccountsContract, AccountsContract>();

		return services;
	}

}
