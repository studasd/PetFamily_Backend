using PetFamily.API.Responses;

namespace PetFamily.API.Middlewares;

public class ExceptionMiddleware
{
	private readonly RequestDelegate next;
	private readonly ILogger<ExceptionMiddleware> logger;

	public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
	{
		this.next = next;
		this.logger = logger;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await next(context);
		}
		catch (Exception ex) 
		{
			logger.LogError(ex, ex.Message);

			var responseError = new ResponseError("server_internal", ex.Message, null);
			var envelope = Envelope.Error([responseError]);

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = StatusCodes.Status500InternalServerError;

			await context.Response.WriteAsJsonAsync(envelope);
		}
	}
}
