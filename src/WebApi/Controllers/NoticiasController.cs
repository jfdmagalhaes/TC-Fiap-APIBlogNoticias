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

    [Authorize]
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
        return await _repository.GetNoticiaById(noticiaId);
    }

    [Authorize]
    [HttpPost("AddNoticia")]
    public async Task<IActionResult> AddNoticiaAsync(Noticia noticia)
    {
        await _repository.AddNoticia(noticia);
        await _repository.UnitOfWork.CommitAsync();

        return Ok();
    }
}