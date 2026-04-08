using Microsoft.AspNetCore.Mvc;

namespace GestionTurnosUTN.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TurnController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello World");
    }
}
