using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Repositories;
public interface INoticiaRepository : IRepository<Noticia>
{

    Task AdicionaNoticia(Noticia noticia);
}