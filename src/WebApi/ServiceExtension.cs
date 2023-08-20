using Infrastructure.EntityFramework.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace WebApi;

public static class ServiceExtension
{
    public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfig = configuration.GetSection("jwtConfig");
        var secretKey = jwtConfig["secret"];
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfig["validIssuer"],
                ValidAudience = jwtConfig["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };
        });
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Blog de noticias",
                Version = "v1",
                Description = "API de blog de noticias",
                Contact = new OpenApiContact
                {
                    Name = "Ajide Habeeb."
                },
            });
            c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });
    }

    public static void MigrationInitialization(IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {

            var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Defina um limite de tempo para a espera pelo banco de dados (em segundos).
            var maxAttempts = 10;
            var currentAttempt = 0;

            while (currentAttempt < maxAttempts)
            {
                try
                {
                    // Tente acessar o banco de dados fazendo uma consulta simples.
                    var canConnect = dbContext.Database.CanConnect();

                    if (canConnect)
                    {
                        // O banco de dados está disponível; execute migrações.
                        dbContext.Database.Migrate();
                        break;
                    }
                }
                catch (Exception ex)
                {
                    // Trate exceções, se necessário.
                    Console.WriteLine($"Tentativa {currentAttempt + 1}: {ex.Message}");
                }

                // Aguarde um curto período de tempo antes de tentar novamente.
                var delayInSeconds = 5;
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(delayInSeconds));

                currentAttempt++;
            }

            if (currentAttempt == maxAttempts)
            {
                Console.WriteLine("O banco de dados não está disponível após várias tentativas.");
                // Lidar com o erro ou registrar, se necessário.
            }

            //serviceScope.ServiceProvider.GetService<ApplicationDbContext>().Database.Migrate();
        }
    }
}
