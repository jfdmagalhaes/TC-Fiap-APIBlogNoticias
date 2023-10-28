using AutoFixture;
using BlogNoticias.IntegrationTests.Helpers;
using Domain.Entities;
using Domain.Repositories;
using Moq;
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

}
