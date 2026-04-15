using GestionTurnosUTN.Application.Dtos;
using GestionTurnosUTN.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GestionTurnosUTN.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TurnController : ControllerBase
{
    private readonly ITurnManagementService _turnService;
    public TurnController(ITurnManagementService turnService)
    {
        _turnService = turnService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello World");
    }
    [HttpPost]
    public async Task<IActionResult> AddTurn([FromBody] TurnModel.Request request)
    {
        var newTurn= await _turnService.CreateTurnAsync(request);
        return Ok(newTurn);
    }
}
