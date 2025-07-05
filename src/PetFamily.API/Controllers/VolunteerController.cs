using Microsoft.AspNetCore.Mvc;
using PetFamily.Domain.VolunteerEntities;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteerController : ControllerBase
{

	[HttpPost]
	public async Task<IActionResult> Create()
	{
		var volonteer = Volunteer.Create("firsname", "lastname", "surname", "email@email.mm", "description", 1, "7777777777");

		if(volonteer.IsFailure)
			return StatusCode(volonteer.Error.TypeCode, volonteer.Error);

		return Ok(volonteer.Value);
	}
}
