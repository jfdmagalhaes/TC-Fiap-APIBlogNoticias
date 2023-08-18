using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Repositories;
public interface INoticiaRepository : IRepository<Noticia>
{
    Task AddNoticia(Noticia noticia);
    Task<IEnumerable<Noticia>> GetAllNoticias();
    Task<Noticia> GetNoticiaById(int noticiaId);
}