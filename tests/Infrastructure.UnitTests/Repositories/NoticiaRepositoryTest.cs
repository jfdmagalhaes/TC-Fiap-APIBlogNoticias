using Domain.Entities;
using FluentAssertions;
using Infrastructure.EntityFramework.Context;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework.Internal;

namespace Infrastructure.UnitTests.Repositories;

[TestFixture]
public class NoticiaRepositoryTest
{
    private NoticiaRepository _repository;
    private Mock<INoticiaDbContext> _context;

    [SetUp]
    public void SetUp()
    {
        _context = new Mock<INoticiaDbContext>();
        _repository = new NoticiaRepository(_context.Object);
    }

    [Test]
    public async Task GetAllNoticias_ShouldReturn_AllNoticias()
    {
        // Arrange
        var noticias = new List<NoticiaDto>
        {
            new NoticiaDto { Id = 1, Titulo = "Noticia 1" },
            new NoticiaDto { Id = 2, Titulo = "Noticia 2" }
        };

        //_context.Setup(x => x.Noticias.ToListAsync())
        //    .ReturnsAsync(noticias);

        // Act
        var result = await _repository.GetAllNoticias();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().ContainEquivalentOf(noticias[0]);
        result.Should().ContainEquivalentOf(noticias[1]);
    }
}