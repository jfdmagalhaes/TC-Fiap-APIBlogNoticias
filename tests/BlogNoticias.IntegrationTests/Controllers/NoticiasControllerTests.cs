using AutoFixture;
using BlogNoticias.IntegrationTests.Helpers;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.EntityFramework.Context;
using Infrastructure.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;
using Xunit;

namespace BlogNoticias.IntegrationTests.Controllers;
public class NoticiasControllerTests : IClassFixture<DockerFixture> 
{
    private readonly HttpClient _httpClient;
    private readonly Mock<INoticiaRepository> _repository = new();
    private readonly DockerFixture _dockerFixture;

    //public NoticiasControllerTests(ApiWebApplicationFactory application)
    //{
    //    _httpClient = application.CreateClient();
    //}

    public NoticiasControllerTests(DockerFixture dockerFixture)
    {
        _dockerFixture = dockerFixture;

        using (var connection = new SqlConnection(_dockerFixture.GetConnectionString()))
        {


            string createTableQuery = @"
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
        }
    }


    [Fact]
    public async void Should_Insert_Fi_With_Success()
    {
        // Arrange
        using (var dbContext = _dockerFixture.CreateDbContext())
        {
            var repository = new NoticiaRepository(dbContext);

            var noticia = new NoticiaDto
            {
               Descricao = "Descricao"
            };

            // Act
            await repository.AddNoticia(noticia);
            await repository.UnitOfWork.CommitAsync();

            // Assert
            var insertedNoticia = await dbContext.Noticias.FindAsync(noticia.Id);
            Assert.NotNull(insertedNoticia);
            // Faça outras asserções de acordo com sua lógica de inserção e recuperação de dados
        }

    }



    //[Fact]
    //public async Task GetAll_ShouldExecute_Successfull()
    //{
    //    // Arrange
    //    var fixture = new Fixture();
    //    var noticias = fixture.CreateMany<NoticiaDto>().ToList();

    //    _repository.Setup(repo => repo.GetAllNoticias()).ReturnsAsync(noticias);

    //    // Act
    //    var response = await _httpClient.GetAsync("/api/Noticias/GetAll");

    //    // Assert
    //    response.EnsureSuccessStatusCode();

    //}

    //[Fact]
    //public async Task GetById_ShouldExecute_Successfull()
    //{
    //    // Arrange
    //    var fixture = new Fixture();
    //    var noticia = fixture.Create<NoticiaDto>();

    //    _repository.Setup(repo => repo.GetNoticiaById(noticia.Id)).ReturnsAsync(noticia);

    //    // Act
    //    var response = await _httpClient.GetAsync($"/api/Noticias/{noticia.Id}");

    //    // Assert
    //    response.EnsureSuccessStatusCode();

    //}

    //[Fact]
    //public async Task DeleteNoticia_ShouldExecute_Successfull()
    //{
    //    // Arrange
    //    var fixture = new Fixture();
    //    var noticia = fixture.Create<NoticiaDto>();

    //    _repository.Setup(repo => repo.DeleteById(noticia.Id));

    //    // Act
    //    var content = new StringContent(JsonConvert.SerializeObject(noticia), Encoding.UTF8, "application/json");
    //    var response = await _httpClient.PutAsync($"/api/Noticias/{noticia.Id}", content);

    //    // Assert
    //    response.EnsureSuccessStatusCode();
    //}

}
