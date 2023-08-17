using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Infrastructure.EntityFramework.Context;
public interface INoticiaDbContext : IUnitOfWork
{
    IDbConnection Connection { get; }
    DbSet<Noticia> Noticias { get; }
}