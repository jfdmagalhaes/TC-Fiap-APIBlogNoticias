using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class NoticiasController : ControllerBase
{
    [HttpGet("GetAll")]
    public IEnumerable<WeatherForecast> GetAllAsync()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
        })
        .ToArray();
    }

    [Authorize]
    [HttpGet("GetById")]
    public IEnumerable<WeatherForecast> GetByIdAsync()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
        })
        .ToArray();
    }
}
