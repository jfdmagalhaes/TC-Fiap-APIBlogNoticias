using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Reflection;

namespace Infrastructure.EntityFramework.Context;
public class NoticiaDbContext : DbContext, INoticiaDbContext
{
    public IDbConnection Connection => base.Database.GetDbConnection();
    private readonly IDbContextTransaction _currentTransaction;
    public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

    public DbSet<NoticiaDto> Noticias { get; set; }

    public NoticiaDbContext()
    {
            
    }
    public NoticiaDbContext(DbContextOptions<NoticiaDbContext> options)
    : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ////These configurations are useful while debuging the code. DON'T USE IN PRODUCTION
        //optionsBuilder
        //.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
        //.EnableDetailedErrors()
        //.EnableSensitiveDataLogging();

        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
            "Server=sqlserver;Database=db_noticias;User=sa;Password=Pass@word;TrustServerCertificate=True;");
        }

    }

    public async Task<bool> CommitAsync()
    {
        return await base.SaveChangesAsync() > 0;
    }
}