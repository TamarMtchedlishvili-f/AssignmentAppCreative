using AssignmentAppCreative.WebAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentAppCreative.WebAPI.Controllers;

[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    readonly IWeatherDataRetriever _weatherDataRetriever;
    public WeatherController(IWeatherDataRetriever weatherDataRetriever) 
        => _weatherDataRetriever = weatherDataRetriever;

    [HttpGet("{cityName}")]
    public  Task<string?> GetWeatherForFirstCity(string cityName) 
        => _weatherDataRetriever.GetWeatherForFirstCity(cityName);
}