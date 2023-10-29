using Docker.DotNet;
using Docker.DotNet.Models;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace BlogNoticias.IntegrationTests.Helpers;
public class DockerFixture : IDisposable
{
    private DockerClient _dockerClient;
    private string _containerId;

    public DockerFixture()
    {
        _dockerClient = new DockerClientConfiguration().CreateClient();

        var createContainerResponse = _dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
        {
            Image = "mcr.microsoft.com/mssql/server:2019-latest",
            Env = new List<string> { "SA_PASSWORD=Pass@word" },
            HostConfig = new HostConfig
            {
                PortBindings = new Dictionary<string, IList<PortBinding>>()
                {
                    { "1433/tcp", new List<PortBinding> { new PortBinding { HostPort = "1433" } } }
                },
                PublishAllPorts = true // Optional: Set this to true if you want to publish all exposed ports
            }
        }).GetAwaiter().GetResult();


        _containerId = createContainerResponse.ID;
        _dockerClient.Containers.StartContainerAsync(_containerId, new ContainerStartParameters()).GetAwaiter().GetResult();
    }

    public string GetConnectionString()
    {
        var _connectionString = $"Server=localhost,1433;Database=db_noticiastest;User=sa;Password=Pass@word;";
        return _connectionString;
    }

    public NoticiaDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<NoticiaDbContext>()
            .UseSqlServer(GetConnectionString())
            .Options;

        return new NoticiaDbContext(options);
    }

    public void Dispose()
    {
        _dockerClient.Containers.StopContainerAsync(_containerId, new ContainerStopParameters()).GetAwaiter().GetResult();
        _dockerClient.Containers.RemoveContainerAsync(_containerId, new ContainerRemoveParameters()).GetAwaiter().GetResult();
        _dockerClient.Dispose();
    }
}