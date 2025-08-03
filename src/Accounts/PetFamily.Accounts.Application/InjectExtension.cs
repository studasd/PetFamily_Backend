using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application;

public static class InjectExtension
{
	public static IServiceCollection AddAccountsApplication(this IServiceCollection services)
	{
		var assembly = typeof(InjectExtension).Assembly;

		services.Scan(scan => scan.FromAssemblies(assembly)
			.AddClasses(classes => classes
			.AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
			.AsSelfWithInterfaces()
			.WithScopedLifetime());

		services.Scan(scan => scan.FromAssemblies(assembly)
			.AddClasses(classes => classes
			.AssignableTo(typeof(IQueryHandler<,>)))
			.AsSelfWithInterfaces()
			.WithScopedLifetime());

		services.AddValidatorsFromAssembly(assembly);

		return services;
	}
}
