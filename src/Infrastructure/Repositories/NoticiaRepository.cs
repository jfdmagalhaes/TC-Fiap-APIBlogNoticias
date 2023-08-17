using Domain.Entities;
using Domain.Interfaces;
using Domain.Repositories;
using Infrastructure.EntityFramework.Context;

namespace Infrastructure.Repositories;
public class NoticiaRepository : INoticiaRepository
{
    private readonly INoticiaDbContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public NoticiaRepository(INoticiaDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task AdicionaNoticia(Noticia noticia)
    {
        await _context.Noticias.AddAsync(noticia);
    }

    private bool disposedValue = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);

    }
}
