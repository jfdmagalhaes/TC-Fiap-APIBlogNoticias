using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Repositories;
public interface INoticiaRepository : IRepository<NoticiaDto>
{
    Task AddNoticia(NoticiaDto noticia);
    Task<IEnumerable<NoticiaDto>> GetAllNoticias();
    Task<NoticiaDto> GetNoticiaById(int noticiaId);
    Task DeleteById(int noticiaId);
}