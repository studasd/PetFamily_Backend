using Microsoft.AspNetCore.Mvc;
using PetFamily.Domain.Shared;

namespace PetFamily.API.Extensions;

public static class ResponseExtensions
{
	public static ActionResult ToErrorResponse(this Error error)
	{
		throw new NotImplementedException();
	}
}
