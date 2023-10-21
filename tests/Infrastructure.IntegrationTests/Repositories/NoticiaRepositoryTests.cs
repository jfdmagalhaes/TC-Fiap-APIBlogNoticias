using Application.IntegrationTests;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Repositories;
using FluentAssertions;
using Infrastructure.EntityFramework.Context;
using Infrastructure.Repositories;
using Xunit;

namespace Infrastructure.IntegrationTests.Repositories;

public class NoticiaRepositoryTests : IClassFixture<DatabaseSetup>, IDisposable
{
    private NoticiaDbContext _context;
    private INoticiaRepository _repository;
    private IUnitOfWork _unitOfWork => _context;

    public NoticiaRepositoryTests(DatabaseSetup fixture)
    {
        _context = fixture.DbContext;
        _context.Connection.BeginTransaction();
        _repository = new NoticiaRepository(_context);
    }

    [Fact]
    public async Task GetAllAsync_ShouldBe_GetNoticias()
    {
        // Arrange
        var noticias = new List<NoticiaDto>
        {
            new NoticiaDto { Id = 1, Titulo = "Noticia 1" },
            new NoticiaDto { Id = 2, Titulo = "Noticia 2" }
        };

        _context.Add(noticias);
        _context.SaveChanges();

        var result = await _repository.GetAllNoticias();

        //// Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<NoticiaDto>>();
    }

    public void Dispose()
    {
        _context.Database.RollbackTransaction();
    }
}