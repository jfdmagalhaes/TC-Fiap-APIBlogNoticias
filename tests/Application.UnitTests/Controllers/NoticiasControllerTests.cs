using Domain.Entities;
using Domain.Interfaces;
using Domain.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using WebApi.Controllers;

namespace Application.UnitTests.Controllers;

[TestFixture]
public class NoticiasControllerTests
{
    private readonly NoticiasController _noticiasController;
    private readonly Mock<INoticiaRepository> _repository = new();

    public NoticiasControllerTests()
    {
        _repository.SetupGet(p => p.UnitOfWork).Returns(new Mock<IUnitOfWork>().Object);
        _noticiasController = new NoticiasController(_repository.Object);
    }

    [Test]
    public async Task GetAll_ShouldExecute_Successfully()
    {
        //arr
        var getList = new List<NoticiaDto> { new NoticiaDto { Autor = "teste"} };
        _repository.Setup(x => x.GetAllNoticias()).ReturnsAsync(getList);

        //act
        var getNoticias = await _noticiasController.GetAllAsync();

        //ass
        Assert.That(getNoticias, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task GetAll_Should_ThrowsException()
    {
        _repository.Reset();

        // Act
        var result = await _noticiasController.GetAllAsync();

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task GetById_ShouldExecute_Successfully()
    {
        //arr
        var noticia = new NoticiaDto { Autor = "teste", Id = 1, Descricao = "teste", Titulo = "Titulo" };
        _repository.Setup(x => x.GetNoticiaById(noticia.Id)).ReturnsAsync(noticia);

        //act
        var getNoticiaById = await _noticiasController.GetByIdAsync(noticia.Id);

        //ass
        Assert.That(getNoticiaById, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task GetById_Should_ThrowsException()
    {
        _repository.Reset();

        // Act
        var result = await _noticiasController.GetByIdAsync(1);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task AddNoticia_ShouldExecute_Successfully()
    {
        //arr
        var noticia = new NoticiaDto { Autor = "teste", Id = 1, Descricao = "teste", Titulo = "Titulo" };
        _repository.Setup(x => x.AddNoticia(noticia));

        //act
        var addNoticiaReponse = await _noticiasController.AddNoticiaAsync(noticia);

        //ass
        addNoticiaReponse.Should().NotBeNull();
        addNoticiaReponse.Should().BeOfType<OkResult>();
    }

    [Test]
    public async Task DeleteNoticia_ShouldExecute_Successfully()
    {
        //arr
        var noticia = new NoticiaDto { Autor = "teste", Id = 1, Descricao = "teste", Titulo = "Titulo" };
        _repository.Setup(x => x.AddNoticia(noticia));
        _repository.Setup(x => x.GetNoticiaById(noticia.Id)).ReturnsAsync(noticia);

        //act
        var addNoticiaReponse = await _noticiasController.DeleteByIdAsync(noticia.Id);

        //ass
        addNoticiaReponse.Should().NotBeNull();
        addNoticiaReponse.Should().BeOfType<OkResult>();
    }
}