using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;

namespace PetFamily.Application;

public static class InjectExtension
{
	public static IServiceCollection AddContracts(this IServiceCollection services)
	{

		services.Scan(scan =>
		{
			scan.FromAssemblies(typeof(InjectExtension).Assembly)
				.AddClasses(c => c.AssignableToAny([typeof(ICommandHandler<,>), typeof(ICommandHandler<>)]))
				.AsSelfWithInterfaces()
				.WithScopedLifetime();
		});

		services.Scan(scan =>
		{
			scan.FromAssemblies(typeof(InjectExtension).Assembly)
				.AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
				.AsSelfWithInterfaces()
				.WithScopedLifetime();
		});


		services.AddValidatorsFromAssembly(typeof(InjectExtension).Assembly);

		return services;
	}
}
