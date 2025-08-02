using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Responses;
using PetFamily.Core.Errores;

namespace PetFamily.API.Extensions;

public static class ResponseExtensions
{
	public static ActionResult ToResponse(this Error error)
	{
		var responseError = new ResponseError(error.Code, error.Message, null);

		var envelope = Envelope.Error(error.ToErrorList());

		return new ObjectResult(envelope) { StatusCode = error.TypeCode };
	}


	public static ActionResult ToResponse(this ErrorList errors)
	{
		if (!errors.Any())
		{
			return new ObjectResult(Envelope.Error(errors))
			{
				StatusCode = StatusCodes.Status500InternalServerError
			};
		}

		var distinctErrorTypes = errors.Select(x => x.TypeCode).Distinct().ToList();

		var statusCode = distinctErrorTypes.Count() > 1 
			? StatusCodes.Status500InternalServerError 
			: distinctErrorTypes.First();

		var envelope = Envelope.Error(errors);

		return new ObjectResult(envelope) { StatusCode = statusCode };
	}
}
