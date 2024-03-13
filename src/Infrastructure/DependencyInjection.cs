using Domain.Repositories;
using Infrastructure.EntityFramework.Context;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infraestructure;
public static class DependencyInjection
{
    public static IServiceCollection AddDataDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));

        services.AddTransient<IUserAuthenticationRepository, UserAuthenticationRepository>();
        services.AddTransient<INoticiaRepository, NoticiaRepository>();

        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? configuration.GetConnectionString("DefaultConnection");
        if (connectionString is null) throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options => { options.UseSqlServer(connectionString); });
        services.AddDbContext<INoticiaDbContext, NoticiaDbContext>(options => { options.UseSqlServer(connectionString); });

        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}