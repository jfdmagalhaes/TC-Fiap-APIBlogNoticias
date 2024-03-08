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


    [HttpGet("Test")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTeste()
    {
        return Ok("ol�");
    }


    [HttpGet("GetAll")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllAsync()
    {
        var noticias = await _repository.GetAllNoticias();
        if (noticias.Any() is false) return NotFound("N�o foi encontrada nenhuma noticia");

        return Ok(noticias);
    }

    [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var noticia = await _repository.GetNoticiaById(id);
        if (noticia == null) return NotFound($"N�o foi encontrada noticia com o Id: {id}");

        return Ok(noticia);
    }

    [HttpPost("AddNoticia")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

    [HttpDelete("DeleteById/{id}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteByIdAsync(int id)
    {
        try
        {
            await _repository.DeleteById(id);
            await _repository.UnitOfWork.CommitAsync();

            return Ok();
        }
        catch (Exception)
        {
            throw;
        }
    }
}