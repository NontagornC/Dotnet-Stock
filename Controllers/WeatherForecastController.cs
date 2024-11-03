using dotnet_stock.Data;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_stock.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    public DatabaseContext DatabaseContext { get; }

    public WeatherForecastController(ILogger<WeatherForecastController> logger, DatabaseContext databaseContext)
    {
        this.DatabaseContext = databaseContext;
        _logger = logger;
    }

    [HttpGet("Test")]
    public IActionResult GetTModel()
    {
        var result = this.DatabaseContext.Products.ToList();
        return Ok(result);
    }


    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
