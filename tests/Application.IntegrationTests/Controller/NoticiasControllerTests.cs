using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using Moq;
using WebApi.Controllers;

namespace Application.IntegrationTests.Controller;

[TestFixture]
public class NoticiasControllerTests
{
    private NoticiasController _controller;
    private Mock<INoticiaRepository> _repositoryMock;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<INoticiaRepository>();
        _controller = new NoticiasController(_repositoryMock.Object);
    }

    [Test]
    public async Task GetAllAsync_ShouldBe_GetNoticias()
    {
        // Arrange
        var noticias = new List<NoticiaDto>
            {
                new NoticiaDto { Id = 1, Titulo = "Noticia 1" },
                new NoticiaDto { Id = 2, Titulo = "Noticia 2" }
            };

        _repositoryMock.Setup(repo => repo.GetAllNoticias()).ReturnsAsync(noticias);

        // Act
        var result = await _controller.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<NoticiaDto>>();
        result.Should().HaveCount(2);
    }
}