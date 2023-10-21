using Application.IntegrationTests;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Repositories;
using FluentAssertions;
using Infrastructure.EntityFramework.Context;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Infrastructure.IntegrationTests.Repositories;

public class NoticiaRepositoryTests : IClassFixture<DatabaseSetup>, IDisposable
{
    private NoticiaDbContext _context;
    private INoticiaRepository _repository;

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

    [Fact]
    public async Task AddNoticia_ShouldBe_Successful()
    {
        //Arrange
        var noticia = new NoticiaDto { Id = 1, Titulo = "Noticia 1" };

        //Act
        await _repository.AddNoticia(noticia);
        await _context.SaveChangesAsync();


        //Assert
        var addedNoticia = await _context.Noticias.FirstOrDefaultAsync(n => n.Id == noticia.Id);
        Assert.NotNull(addedNoticia);
    }

    [Fact]
    public async Task DeleteNoticiaById_ShouldBe_Successful()
    {
        //Arrange
        var noticia = new NoticiaDto { Id = 1, Titulo = "Noticia 1" };

        //Act
        await _repository.DeleteById(noticia.Id);
        await _context.SaveChangesAsync();

        //Assert
        var getNoticia = await _context.Noticias.FirstOrDefaultAsync(n => n.Id == noticia.Id);
        Assert.Null(getNoticia);
    }

    [Fact]
    public async Task GetNoticiaById_ShouldBe_GetNoticia()
    {
        // Arrange
        var noticia = new NoticiaDto { Id = 1, Titulo = "Noticia 1" };

        _context.Add(noticia);
        _context.SaveChanges();

        var result = await _repository.GetNoticiaById(noticia.Id);

        //// Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<NoticiaDto>();
    }

    public void Dispose()
    {
        _context.Database.RollbackTransaction();
    }
}