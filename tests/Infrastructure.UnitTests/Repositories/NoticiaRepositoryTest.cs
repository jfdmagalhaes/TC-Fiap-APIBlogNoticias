using Domain.Entities;
using FluentAssertions;
using Infrastructure.EntityFramework.Context;
using Infrastructure.Repositories;
using Infrastructure.UnitTests.Repositories.Utils;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework.Internal;

namespace Infrastructure.UnitTests.Repositories;

[TestFixture]
public class NoticiaRepositoryTest
{
    private NoticiaRepository _repository;
    private Mock<INoticiaDbContext> _context;
    private readonly List<NoticiaDto> noticias = new();

    [SetUp]
    public void SetUp()
    {
        noticias.AddRange(UnitTestTools.CreateNoticias());

        var noticiasMock = noticias.AsQueryable().BuildMockDbSet();

        _context = new Mock<INoticiaDbContext>();
        _context.Setup(x => x.Noticias).Returns(noticiasMock.Object);
        _repository = new NoticiaRepository(_context.Object);
    }

    [Test]
    public async Task GetAllNoticias_ShouldReturn_AllNoticias()
    {
        // Act
        var result = await _repository.GetAllNoticias();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<NoticiaDto>>();
    }

    [Test]
    public void AddNoticia_ShouldAdd_Successfully()
    {
        //arr
        var noticia = UnitTestTools.CreateNoticia();

        // Act & Ass
        Assert.DoesNotThrowAsync(async () => await _repository.AddNoticia(noticia));
    }

    [Test]
    public void DeleteNoticia_ShouldDelete_Successfully()
    {
        //arr
        var noticia = UnitTestTools.CreateNoticia();

        // Act & Ass
        Assert.DoesNotThrowAsync(async () => await _repository.DeleteById(noticia.Id));
    }

    [Test]
    public void Dispose()
        => Assert.DoesNotThrow(() => _repository.Dispose());
}