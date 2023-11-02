using Infraestructure;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace WebApi;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddDataDependencyInjection(Configuration);

        services.ConfigureJWT(Configuration);
        services.ConfigureSwagger();

        services.AddRazorPages();
        services.AddApplicationInsightsTelemetry();

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
            });
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapRazorPages();
        });

        using (var scope = app.ApplicationServices.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>(); // Substitua YourDbContext pelo nome do seu DbContext.

            if (dbContext.Database.IsRelational())
                ServiceExtension.MigrationInitialization(app);


        }
    }
}