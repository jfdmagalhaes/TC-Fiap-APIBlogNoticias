using Domain.Entities;
using Domain.Repositories;
using Infrastructure.EntityFramework.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.Text;
using WebApi;

namespace BlogNoticias.IntegrationTests.Helpers;
public class ApiWebApplicationFactory : WebApplicationFactory<Startup> 
{
    public IConfiguration Configuration { get; private set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var root = new InMemoryDatabaseRoot();

        builder.ConfigureServices((context, services) =>
        {
            services.AddDbContext<NoticiaDbContext>(options =>
            {
                options.UseInMemoryDatabase("db_noticias", root);
            });

            base.ConfigureWebHost(builder);


            var repositoryFake = new Mock<INoticiaRepository>();
            repositoryFake.Setup(repo => repo.GetAllNoticias()).ReturnsAsync(new List<NoticiaDto>());
            services.AddSingleton(repositoryFake.Object);


            var secretKey = "ABCDEFGHIJKLMNOP";

            // Configurar o serviço de autenticação para simular autorizações
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });

            base.ConfigureWebHost(builder);

        });
      
    }
}