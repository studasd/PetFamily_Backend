using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Responses;
using PetFamily.Domain.Shared;

namespace PetFamily.API.Extensions;

public static class ResponseExtensions
{
	public static ActionResult ToResponse(this Error error)
	{
		var responseError = new ResponseError(error.Code, error.Message, null);

		var envelope = Envelope.Error([responseError]);

		return new ObjectResult(envelope) { StatusCode = error.TypeCode };
	}


	public static ActionResult ToValidationErrorResponse(this ValidationResult result)
	{
		if (result.IsValid)
			throw new InvalidOperationException("Result can note be succeed");

		var validationErrors = result.Errors;

		var responseErrors = from validationError in validationErrors
							 let errorMessage = validationError.ErrorMessage
							 let error = Error.Deserialize(errorMessage)
							 select new ResponseError(error.Code, error.Message, validationError.PropertyName);

		var envelope = Envelope.Error(responseErrors);

		return new ObjectResult(envelope) { StatusCode = StatusCodes.Status400BadRequest };
	}
}
