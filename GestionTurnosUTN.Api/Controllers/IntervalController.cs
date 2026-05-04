using Microsoft.AspNetCore.Mvc;
namespace GestionTurnosUTN.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using GestionTurnosUTN.Application.Interfaces;
using GestionTurnosUTN.Application.Dtos;
using GestionTurnosUTN.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class IntervalController : ControllerBase
{
    private readonly IIntervalService _intervalService;

    public IntervalController(IIntervalService intervalService)
    {
        _intervalService = intervalService;
    }

    // 🔹 GET ALL
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _intervalService.GetAllAsync();
        return Ok(result);
    }

    // 🔹 GET BY ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _intervalService.GetByIdAsync(id);

        if (result == null)
            return NotFound("Intervalo no encontrado");

        return Ok(result);
    }

    // 🔹 CREATE
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] IntervalCreateDTO dto)
    {
        var result = await _intervalService.CreateAsync(dto);
        return Ok(result);
    }

    // 🔹 UPDATE
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] IntervalUpdateDTO dto)
    {
        var result = await _intervalService.UpdateAsync(dto);
        return Ok(result);
    }

    // 🔹 DEACTIVATE
    [HttpPut("deactivate")]
    public async Task<IActionResult> Deactivate([FromBody] IntervalDeactivateDTO dto)
    {
        await _intervalService.DeactivateAsync(dto);
        return Ok("Intervalo desactivado correctamente");
    }
}


