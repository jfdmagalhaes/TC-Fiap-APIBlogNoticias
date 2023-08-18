using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class NoticiasController : ControllerBase
{
    private INoticiaRepository _repository;

    public NoticiasController(INoticiaRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    [HttpGet("GetAll")]
    public async Task<IEnumerable<Noticia>> GetAllAsync()
    {
        var noticias = await _repository.GetAllNoticias();
        if (noticias == null) throw new ArgumentNullException("Não foi encontrada nenhuma noticia");

        return await _repository.GetAllNoticias();
    }

    [Authorize]
    [HttpGet("GetById")]
    public async Task<Noticia> GetByIdAsync(int noticiaId)
    {
        var noticia = await _repository.GetNoticiaById(noticiaId);
        if (noticia == null) throw new ArgumentNullException($"Não foi encontrada nenhuma noticia com o id {noticiaId}");

        return noticia;
    }

    [HttpPost("AddNoticia")]
    public async Task<IActionResult> AddNoticiaAsync(Noticia noticia)
    {
        await _repository.AddNoticia(noticia);
        return Ok();
    }
}