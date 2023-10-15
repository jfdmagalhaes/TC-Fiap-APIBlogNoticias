using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net;
using WebApi;

namespace Application.IntegrationTests.Controller;

[TestFixture]
public class NoticiasControllerTests
{
    private HttpClient _client;
    private TestServer _server;

    [SetUp]
    public void Setup()
    {
        _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
        _client = _server.CreateClient();
    }

    [Test]
    public async Task GetAllAsync_Returns_NotFound()
    {
        // Arrange
        var requestUri = "/api/Noticias/GetAll";

        // Act
        var response = await _client.GetAsync(requestUri);

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }
}
