using Microsoft.AspNetCore.Mvc;
using PetFamily.Domain.VolunteerEntities;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{

	[HttpGet]
	public async Task<IActionResult> Add()
	{
		var volonteer = Volunteer.Create("firsname", "lastname", "surname", "email@email.mm", "description", 1, "7777777777");

		if(volonteer.IsFailure)
			return StatusCode(volonteer.Error.TypeCode, volonteer.Error);

		return Ok(volonteer.Value);
	}
}
