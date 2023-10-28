using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Application.IntegrationTests;
public class DatabaseSetup
{
    public NoticiaDbContext DbContext { get; private set; }

    public DatabaseSetup()
    {
        DbContext = CreateDbContext();
    }

    private NoticiaDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<NoticiaDbContext>()
            .UseInMemoryDatabase(databaseName: "BlogNewsInMemory")
            .EnableSensitiveDataLogging()
            .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new NoticiaDbContext(options);

        context.Database.EnsureCreated();

        return context;
    }
}
