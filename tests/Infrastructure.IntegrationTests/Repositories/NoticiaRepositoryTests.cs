using Application.IntegrationTests;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Repositories;
using FluentAssertions;
using Infrastructure.EntityFramework.Context;
using Infrastructure.Repositories;
using NUnit.Framework;

namespace Infrastructure.IntegrationTests.Repositories;

[TestFixture]
public class NoticiaRepositoryTests
{
    private INoticiaDbContext _context = DatabaseSetup.CreateContext();
    private INoticiaRepository _repository;
    private IUnitOfWork _unitOfWork => _context;

    public NoticiaRepositoryTests()
    {
        DatabaseSetup.Seed();        
    }

    [SetUp]
    public void Setup()
    {
        _repository = new NoticiaRepository(_context);
    }

    [TearDown]
    public void Clenup()
    {
        _unitOfWork.CommitAsync();
        _context.Connection.Dispose();
    }

    [Test]
    public async Task GetAllAsync_ShouldBe_GetNoticias()
    {
        var result = await _repository.GetAllNoticias();

        //// Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<NoticiaDto>>();
    }
}