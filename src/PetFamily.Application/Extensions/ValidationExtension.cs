using FluentValidation.Results;
using PetFamily.Core.Errores;

namespace PetFamily.Application.Extensions;

public static class ValidationExtension
{
	public static ErrorList ToErrorList(this ValidationResult validationResult)
	{
		var errors = from validationError in validationResult.Errors
					 let errorMessage = validationError.ErrorMessage
					 let error = Error.Deserialize(errorMessage)
					 select Error.Validation(error.Code, error.Message, validationError.PropertyName);

		return errors.ToList();
	}
}
