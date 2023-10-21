using Domain.Entities;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application.IntegrationTests;
public class DatabaseSetup : IDisposable
{
    public NoticiaDbContext DbContext { get; private set; }

    public DatabaseSetup()
    {
        DbContext = CreateDbContext();
    }

    public void Dispose()
    {
        DbContext.Dispose();
        DbContext.Database.RollbackTransaction();
    }

    private NoticiaDbContext CreateDbContext()
    {
        var host = Host.CreateDefaultBuilder().Build();
        var config = host.Services.GetRequiredService<IConfiguration>();

        //var options = new DbContextOptionsBuilder<NoticiaDbContext>()
        //    .UseSqlServer(config.GetConnectionString("DefaultConnectionTest"))
        //    .Options;


        var optionBuilder = new DbContextOptionsBuilder<NoticiaDbContext>();
        optionBuilder.UseSqlServer(
            config.GetConnectionString("DefaultConnectionTest"),
            optionsAction =>
            {
                optionsAction.EnableRetryOnFailure(10, TimeSpan.FromSeconds(10), null);
            });


        var context = new NoticiaDbContext(optionBuilder.Options);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        return context;
    }
}
