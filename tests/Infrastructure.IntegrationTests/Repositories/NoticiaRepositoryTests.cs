using Application.IntegrationTests;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Repositories;
using Infrastructure.EntityFramework.Context;
using Infrastructure.Repositories;
using NUnit.Framework;

namespace Infrastructure.IntegrationTests.Repositories;

[TestFixture]
public class NoticiaRepositoryTests
{
    private INoticiaDbContext _context;
    private INoticiaRepository _repository;
    private IUnitOfWork? _unitOfWork;

    public NoticiaRepositoryTests()
    {
        DatabaseSetup.Seed();        
    }

    [SetUp]
    public void Setup()
    {
        _context = DatabaseSetup.CreateContext();
        _repository = new NoticiaRepository(_context);
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


        var result = await _repository.GetAllNoticias();

        //_repositoryMock.Setup(repo => repo.GetAllNoticias()).ReturnsAsync(noticias);

        //// Act
        //var result = await _controller.GetAllAsync();

        //// Assert
        //result.Should().NotBeNull();
        //result.Should().BeOfType<List<NoticiaDto>>();
        //result.Should().HaveCount(2);
    }
}