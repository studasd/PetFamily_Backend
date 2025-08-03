using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Models;

namespace PetFamily.Core;

[ApiController]
[Route("[controller]")]
public abstract class ApplicationController : ControllerBase
{
	public override OkObjectResult Ok(object? value)
	{
		var envelope = Envelope.Ok(value);

		return base.Ok(envelope);
	}
}
