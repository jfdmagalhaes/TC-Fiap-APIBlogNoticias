using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructure.EntityFramework.Context;
public interface INoticiaDbContext : IUnitOfWork
{
    IDbConnection Connection { get; }
    DbSet<Noticia> Noticias { get; }
}