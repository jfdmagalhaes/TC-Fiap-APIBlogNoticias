using Domain.Entities;
using Domain.Interfaces;
using Domain.Repositories;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class NoticiaRepository : INoticiaRepository
{
    private readonly INoticiaDbContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public NoticiaRepository(INoticiaDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task AddNoticia(Noticia noticia)
    {
        await _context.Noticias.AddAsync(noticia);
    }

    public async Task<IEnumerable<Noticia>> GetAllNoticias()
    {
        return await _context.Noticias.ToListAsync();
    }

    public async Task<Noticia> GetNoticiaById(int noticiaId)
    {
        return await _context.Noticias.Where(x => x.Id == noticiaId).FirstOrDefaultAsync();
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