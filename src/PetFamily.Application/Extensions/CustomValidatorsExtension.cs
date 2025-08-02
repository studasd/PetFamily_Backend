﻿using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Core.Errores;

namespace PetFamily.Application.Extensions;

public static class CustomValidatorsExtension
{
	// Валидация ValueObjects 6-1.43.20
	public static IRuleBuilderOptionsConditions<T, TElement> MustBeValueObject<T, TElement, TValueObject>(this IRuleBuilder<T, TElement> ruleBuilder, Func<TElement, Result<TValueObject, Error>> factoryMethod)
	{
		return ruleBuilder.Custom((value, context) =>
		{
			Result<TValueObject, Error> result = factoryMethod(value);

			if (result.IsSuccess)
				return;

			context.AddFailure(result.Error.Serialize());
		});
	}

	public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, Error error)
	{
		return rule.WithMessage(error.Serialize());
	}
}
