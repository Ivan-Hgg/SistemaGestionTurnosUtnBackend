using GestionTurnosUTN.Application.Dtos;
using GestionTurnosUTN.Application.Interfaces;
using GestionTurnosUTN.Application.Validation;
using GestionTurnosUTN.Data.Identity;
using GestionTurnosUTN.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace GestionTurnosUTN.Api.Controllers;
[ApiController]
[Route("api/auth")]
public class AuthenticateController : ControllerBase
{
        private readonly IAuthenticateService _authenticateService;

    public AuthenticateController(IAuthenticateService authenticateService)
    {
        _authenticateService = authenticateService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModelRequest request)
    {
        var token= await _authenticateService.LoginAsync(request);
        return Ok(token);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterStudent([FromBody] RegisterStudentModel model)
    {
        var result = await _authenticateService.RegisterStudentAsync(model);
        return StatusCode(201, result);
    }
    [HttpPost("worker/register")]
    [Authorize(Roles = "SUPREME")]
    public async Task<IActionResult> RegisterWorker([FromBody] RegisterWorkerModel model)
    {
        var result = await _authenticateService.RegisterWorkerAsync(model);
        return StatusCode(201, result);
    }
    [HttpPatch("worker/register")]
    [Authorize(Roles = "SUPREME")]
    public async Task<IActionResult> ChangeRoleToSupreme([FromBody] Guid workerId)
    {
        await _authenticateService.BeSupreme(workerId);
        return Ok();
    }
}
