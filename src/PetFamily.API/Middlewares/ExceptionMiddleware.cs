using PetFamily.API.Responses;
using PetFamily.Domain.Shared.Errores;

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

			var error = Error.Failure("server_internal", ex.Message);
			var envelope = Envelope.Error(error);

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = StatusCodes.Status500InternalServerError;

			await context.Response.WriteAsJsonAsync(envelope);
		}
	}
}
