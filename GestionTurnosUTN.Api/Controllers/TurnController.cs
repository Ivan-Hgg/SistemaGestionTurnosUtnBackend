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
    [HttpPost]
    public async Task<IActionResult> AddTurn([FromBody] TurnModel.Request request)
    {
        var newTurn= await _turnService.CreateTurnAsync(request);
        return Ok(newTurn);
    }

    [HttpPut]
    public async Task<IActionResult> CancelTurn([FromBody] TurnModel.CancelRequest request)
    {
        await _turnService.CancelTurnAsync(request);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetTurns([FromQuery]TurnModel.FilterTurn request)
    {
        var turns= await _turnService.GetTurnsAsync(request);
        if (turns is null) return NoContent();
        return Ok(turns);
    }
}
