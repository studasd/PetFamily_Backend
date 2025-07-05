using Microsoft.AspNetCore.Mvc;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{

	[HttpGet]
	public async Task<IActionResult> Add()
	{
		return StatusCode(200);
	}
}
