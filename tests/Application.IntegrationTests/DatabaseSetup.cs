using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.IntegrationTests;
public class DatabaseSetup : IDisposable
{
    private static readonly object _lock = new object();
    private static bool _databaseInitialized;
    private string dbName = "TestDatabase.db";

    public static NoticiaDbContext CreateContext()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        var databaseSettings = config.GetConnectionString("DefaultConnection");

        var optionBuilder = new DbContextOptionsBuilder<NoticiaDbContext>();
        optionBuilder.UseSqlServer(
            databaseSettings,
            optionsAction =>
            {
                optionsAction.EnableRetryOnFailure(10, TimeSpan.FromSeconds(10), null);
            });

        return new NoticiaDbContext(optionBuilder.Options);
    }
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
