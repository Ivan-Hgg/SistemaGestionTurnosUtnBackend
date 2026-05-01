using GestionTurnosUTN.Application.Dtos;
using GestionTurnosUTN.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionTurnosUTN.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "WORKER")]
public class NewsController : ControllerBase
{
    private readonly INewsService _newsService;

    public NewsController(INewsService newsService)
    {
        _newsService = newsService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var news = await _newsService.GetAllNewsAsync();
        return Ok(news);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] NewsModel.RequestNewsModel request)
    {
        // Por ahora hardcodeamos el workerId hasta implementar autenticación
        var workerId = Guid.NewGuid();
        await _newsService.CreateNewsAsync(request, workerId);
        return Ok("Noticia creada exitosamente");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] NewsModel.RequestNewsModel request)
    {
        await _newsService.UpdateNewsAsync(id, request);
        return Ok("Noticia actualizada exitosamente");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _newsService.DeleteNewsAsync(id);
        return Ok("Noticia eliminada exitosamente");
    }
}