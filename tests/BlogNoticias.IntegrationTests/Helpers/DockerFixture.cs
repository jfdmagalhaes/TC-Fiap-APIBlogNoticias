using Docker.DotNet;
using Docker.DotNet.Models;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace TestProject1.Helpers;
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
            Env = new List<string> { "SA_PASSWORD=Pass@word", "ACCEPT_EULA=Y" },
            HostConfig = new HostConfig
            {
                PortBindings = new Dictionary<string, IList<PortBinding>>()
                {
                    { "1433/tcp", new List<PortBinding> { new PortBinding { HostPort = "1433" } } }
                },
                PublishAllPorts = true // Optional: Set this to true if you want to publish all exposed ports
            }
        }).GetAwaiter().GetResult();

        RemoveContainersOnTheSamePort();

        _containerId = createContainerResponse.ID;
        _dockerClient.Containers.StartContainerAsync(_containerId, new ContainerStartParameters()).GetAwaiter().GetResult();
    }

    private void RemoveContainersOnTheSamePort()
    {
        var containersListeningOnPort1433 = _dockerClient.Containers.ListContainersAsync(new ContainersListParameters()
        {
            Filters = new Dictionary<string, IDictionary<string, bool>>()
            {
                { "expose", new Dictionary<string, bool>() { { "1433", true } } }
            }
        }).GetAwaiter().GetResult();

        foreach (var container in containersListeningOnPort1433)
        {
            // Remove cada container encontrado
            _dockerClient.Containers.RemoveContainerAsync(container.ID, new ContainerRemoveParameters() { Force = true }).GetAwaiter().GetResult();
        }
    }

    public string GetConnectionString()
    {
        var _connectionString = $"Server=localhost,1433;User=sa;Password=Pass@word;TrustServerCertificate=True;";
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
        _dockerClient.Containers.PruneContainersAsync(new ContainersPruneParameters()).GetAwaiter().GetResult();
        _dockerClient.Dispose();
    }
}