using AutoFixture;
using BlogNoticias.IntegrationTests.Factory;
using BlogNoticias.IntegrationTests.Utils;
using Domain.Entities;
using Infrastructure.EntityFramework.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using WebApi;
using WebApi.Controllers;
using Xunit;

namespace BlogNoticias.IntegrationTests.Controllers;
public class NoticiasControllerTest : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> _application;
    private HttpClient _httpClient;
    private readonly IntegrationTestTools _integrationTestTools;

    public NoticiasControllerTest(CustomWebApplicationFactory<Startup> application)
    {
        _application = application;
        _httpClient = application.CreateClient();
        _integrationTestTools = new IntegrationTestTools(_application);
    }

    [Fact]
    public async Task AddNoticia_Should_ExecuteSuccessfull()
    {
        //Arrange
        _httpClient = await _integrationTestTools.LoginUser();
        var noticia = new NoticiaDto
        {
            Descricao = "Descricao",
            Autor = "Autor",
            DataPublicacao = DateTime.Now,
            Titulo = "Titulo"
        };

        //Act
        var response = await _httpClient.PostAsJsonAsync("/api/Noticias/AddNoticia", noticia);

        //Asserts
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}