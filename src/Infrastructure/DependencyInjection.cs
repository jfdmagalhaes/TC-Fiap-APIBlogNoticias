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

        services.AddDbContext<ApplicationDbContext>(options => { options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")); });
        services.AddDbContext<INoticiaDbContext, NoticiaDbContext>(options => { options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")); });

        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}