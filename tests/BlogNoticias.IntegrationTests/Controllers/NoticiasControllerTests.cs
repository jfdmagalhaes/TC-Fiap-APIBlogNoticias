using AutoFixture;
using BlogNoticias.IntegrationTests.Helpers;
using Domain.Entities;
using Domain.Repositories;
using Moq;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;
using Xunit;

namespace BlogNoticias.IntegrationTests.Controllers;
public class NoticiasControllerTests : IClassFixture<ApiWebApplicationFactory> 
{
    private readonly HttpClient _httpClient;
    private readonly Mock<INoticiaRepository> _repository = new();

    public NoticiasControllerTests(ApiWebApplicationFactory application)
    {
        _httpClient = application.CreateClient();
    }

    [Fact]
    public async Task GetAll_ShouldExecute_Successfull()
    {
        // Arrange
        var fixture = new Fixture();
        var noticias = fixture.CreateMany<NoticiaDto>().ToList();

        _repository.Setup(repo => repo.GetAllNoticias()).ReturnsAsync(noticias);

        // Act
        var response = await _httpClient.GetAsync("/api/Noticias/GetAll");

        // Assert
        response.EnsureSuccessStatusCode();

    }

    [Fact]
    public async Task GetById_ShouldExecute_Successfull()
    {
        // Arrange
        var fixture = new Fixture();
        var noticia = fixture.Create<NoticiaDto>();

        _repository.Setup(repo => repo.GetNoticiaById(noticia.Id)).ReturnsAsync(noticia);

        // Act
        var response = await _httpClient.GetAsync($"/api/Noticias/{noticia.Id}");

        // Assert
        response.EnsureSuccessStatusCode();

    }

    [Fact]
    public async Task DeleteNoticia_ShouldExecute_Successfull()
    {
        // Arrange
        var fixture = new Fixture();
        var noticia = fixture.Create<NoticiaDto>();

        _repository.Setup(repo => repo.DeleteById(noticia.Id));

        // Act
        var content = new StringContent(JsonConvert.SerializeObject(noticia), Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"/api/Noticias/{noticia.Id}", content);

        // Assert
        response.EnsureSuccessStatusCode();
    }

}
