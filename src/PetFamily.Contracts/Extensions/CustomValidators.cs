using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Domain.Shared;

namespace PetFamily.Contracts.Extensions;

public static class CustomValidators
{
	// Валидация ValueObjects 6-1.43.20
	public static IRuleBuilderOptionsConditions<T, TElement> MustBeValueObject<T, TElement, TValueObject>(this IRuleBuilder<T, TElement> ruleBuilder, Func<TElement, Result<TValueObject, Error>> factoryMethod)
	{
		return ruleBuilder.Custom((value, context) =>
		{
			Result<TValueObject, Error> result = factoryMethod(value);

			if (result.IsSuccess)
				return;

			context.AddFailure(result.Error.Message);
		});
	}
}
