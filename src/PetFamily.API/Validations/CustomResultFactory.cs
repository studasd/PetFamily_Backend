namespace PetFamily.API.Validations;

//public class CustomResultFactory : IFluentValidationAutoValidationResultFactory
//{
	//public IActionResult CreateActionResult(
	//	ActionExecutingContext context, 
	//	ValidationProblemDetails? validationProblemDetails)
	//{
	//	if(validationProblemDetails is null)
	//		throw new InvalidOperationException("ValidationProblemDetails is null");

	//	var errors = new List<ResponseError>();

	//	foreach (var (invalideField, validationErrors) in validationProblemDetails.Errors) 
	//	{
	//		var responseErrors = from errorMessage in validationErrors
	//							 let error = Error.Deserialize(errorMessage)
	//							 select new ResponseError(error.Code, error.Message, invalideField);

	//		errors.AddRange(responseErrors);
	//	}

	//	var envelope = Envelope.Error(errors);

	//	return new ObjectResult(envelope)
	//	{
	//		StatusCode = StatusCodes.Status400BadRequest
	//	};
	//}
//}
