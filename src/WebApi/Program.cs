using Infraestructure;
using Infrastructure.EntityFramework.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Validations;
using System.Text;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDataDependencyInjection(builder.Configuration);

builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.ConfigureSwagger();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

//executa as migrations, para criação das tabelas do identity
using var scope = app.Services.CreateScope();
scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();


app.Run();
