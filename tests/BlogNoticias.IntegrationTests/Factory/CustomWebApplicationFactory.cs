using Infrastructure.EntityFramework.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BlogNoticias.IntegrationTests.Factory;
public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    private readonly Dictionary<Type, object> _serviceFakes = new Dictionary<Type, object>();

    public void AddServiceFake<TService>(TService service)
    {
        _serviceFakes[typeof(TService)] = service;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var root = new InMemoryDatabaseRoot();

        builder.ConfigureServices((context, services) =>
        {
            ReplaceServicesWithFakes(services);

            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("db_noticiastest", root);
            });
        });

    }

    private void ReplaceServicesWithFakes(IServiceCollection services)
    {
        foreach (var serviceFake in _serviceFakes)
        {
            services.RemoveAll(serviceFake.Key);
            services.AddSingleton(serviceFake.Key, serviceFake.Value);
        }
    }
}