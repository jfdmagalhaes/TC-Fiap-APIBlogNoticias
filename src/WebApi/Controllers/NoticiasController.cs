using Domain.Entities;
using Domain.Models;
using Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NoticiasController : ControllerBase
{
    private INoticiaRepository _repository;


    public NoticiasController(
        INoticiaRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    [Authorize]
    [HttpGet("GetAll")]
    public async Task<IEnumerable<NoticiaDto>> GetAllAsync()
    {
        var noticias = await _repository.GetAllNoticias();
        if (noticias == null) throw new ArgumentNullException("Não foi encontrada nenhuma noticia");

        return await _repository.GetAllNoticias();
    }

    [Authorize]
    [HttpGet("GetById")]
    public async Task<NoticiaDto> GetByIdAsync(int noticiaId)
    {
        return await _repository.GetNoticiaById(noticiaId);
    }

    [Authorize]
    [HttpPost("AddNoticia")]
    public async Task<IActionResult> AddNoticiaAsync(Noticia noticia)
    {
        try
        {
            var noticiaDto = new NoticiaDto
            {
                Autor = noticia.Autor,
                DataPublicacao = noticia.DataPublicacao,
                Descricao = noticia.Descricao,
                Titulo = noticia.Titulo
            };

            await _repository.AddNoticia(noticiaDto);
            await _repository.UnitOfWork.CommitAsync();

            return Ok();
        }
        catch (Exception) 
        {
            throw; 
        }
    }
}