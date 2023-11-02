using BlogNoticias.IntegrationTests.Infra;
using Domain.Entities;
using Infrastructure.Repositories;
using Microsoft.Data.SqlClient;
using Xunit;

namespace BlogNoticias.IntegrationTests.Repository;

public class NoticiasRepositoryTests : IClassFixture<DockerFixture>
{
    private readonly DockerFixture _dockerFixture;

    public NoticiasRepositoryTests(DockerFixture dockerFixture)
    {
        _dockerFixture = dockerFixture;
        _dockerFixture.StartContainer();

        using (var connection = new SqlConnection(_dockerFixture.GetConnectionString()))
        {
            connection.Open();

            string checkDatabaseQuery = "IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'db_noticiastest') CREATE DATABASE db_noticiastest";
            using (var command = new SqlCommand(checkDatabaseQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            string createTableQuery = @"
                IF OBJECT_ID('dbo.Noticias') IS NOT NULL  
                    DROP TABLE [dbo].Noticias

                CREATE TABLE Noticias (
                    Id INT PRIMARY KEY IDENTITY,
                    Titulo NVARCHAR(255) NOT NULL,
                    Conteudo NVARCHAR(MAX) NOT NULL,
                    DataPublicacao DATETIME NOT NULL,
                    Autor NVARCHAR(255) NOT NULL
            );";

            using (var command = new SqlCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    [Fact]
    public async void Should_InsertNewNoticia_Successfull()
    {
        // Arrange
        using (var dbContext = _dockerFixture.CreateDbContext())
        {
            var repository = new NoticiaRepository(dbContext);

            var noticia = new NoticiaDto
            {
                Descricao = "Descricao",
                Autor = "Autor",
                DataPublicacao = DateTime.Now,
                Titulo = "Titulo"
            };

            // Act
            await repository.AddNoticia(noticia);
            await repository.UnitOfWork.CommitAsync();

            // Assert
            var insertedNoticia = await dbContext.Noticias.FindAsync(noticia.Id);
            Assert.NotNull(insertedNoticia);
        }
    }

    [Fact]
    public async void Should_DeleteNoticia_Successfull()
    {
        // Arrange
        using (var dbContext = _dockerFixture.CreateDbContext())
        {
            var repository = new NoticiaRepository(dbContext);

            var noticia = new NoticiaDto
            {
                Descricao = "Descricao",
                Autor = "Autor",
                DataPublicacao = DateTime.Now,
                Titulo = "Titulo"
            };

            // Act
            await repository.AddNoticia(noticia);
            await repository.UnitOfWork.CommitAsync();

            await repository.DeleteById(noticia.Id);
            await repository.UnitOfWork.CommitAsync();

            // Assert
            var insertedNoticia = await dbContext.Noticias.FindAsync(noticia.Id);
            Assert.Null(insertedNoticia);
        }
    }

    [Fact]
    public async void Should_GetAllNoticias_Successfull()
    {
        // Arrange
        using (var dbContext = _dockerFixture.CreateDbContext())
        {
            var repository = new NoticiaRepository(dbContext);

            var noticias = CreateNoticias();
            // Act
            foreach (var item in noticias)
            {
                await repository.AddNoticia(item);
                await repository.UnitOfWork.CommitAsync();
            }

            var getAll = await repository.GetAllNoticias();

            // Assert
            Assert.NotNull(getAll);
            Assert.Equal(2, getAll.Count());
        }
    }

    private List<NoticiaDto> CreateNoticias()
    {
        var noticias = new List<NoticiaDto>();

        var noticia = new NoticiaDto
        {
            Descricao = "Descricao",
            Autor = "Autor",
            DataPublicacao = DateTime.Now,
            Titulo = "Titulo"
        };

        var noticia2 = new NoticiaDto
        {
            Descricao = "Descricao2",
            Autor = "Autor2",
            DataPublicacao = DateTime.Now,
            Titulo = "Titulo2"
        };

        noticias.Add(noticia);
        noticias.Add(noticia2);
        return noticias;
    }
}