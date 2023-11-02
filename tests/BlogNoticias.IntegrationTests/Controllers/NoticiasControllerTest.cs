using BlogNoticias.IntegrationTests.Factory;
using BlogNoticias.IntegrationTests.Utils;
using Domain.Entities;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using WebApi;
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
    public async Task PostNoticia_Should_ExecuteFailure()
    {
        //Arrange
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
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }


    [Fact]
    public async Task PostNoticia_Should_ExecuteSuccessfull()
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

    [Fact]
    public async Task GelAllNoticias_Should_ExecuteSuccessfull()
    {
        //Arrange
        _httpClient = await _integrationTestTools.LoginUser();
        await _integrationTestTools.AddNoticia(_httpClient);

        //Act
        var response = await _httpClient.GetAsync("/api/Noticias/GetAll");
        var content = await response.Content.ReadAsStringAsync();

        //Asserts
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task DeleteNoticias_Should_ExecuteSuccessfull()
    {
        //Arrange
        _httpClient = await _integrationTestTools.LoginUser();
        await _integrationTestTools.AddNoticia(_httpClient);

        var responseGetAll = await _httpClient.GetAsync("/api/Noticias/GetAll");
        var content = responseGetAll.Content.ReadAsStringAsync().Result;
        var getNoticia = JsonConvert.DeserializeObject<List<NoticiaDto>>(content);
        var noticiaToDelete = getNoticia!.FirstOrDefault();

        //Act
        var response = await _httpClient.DeleteAsync($"/api/Noticias/DeleteById/{noticiaToDelete!.Id}");

        //Asserts
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetById_Should_ExecuteFailure()
    {
        //Arrange
        _httpClient = await _integrationTestTools.LoginUser();

        //Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(async () => await _httpClient.GetAsync($"/api/Noticias/1"));
    }
}