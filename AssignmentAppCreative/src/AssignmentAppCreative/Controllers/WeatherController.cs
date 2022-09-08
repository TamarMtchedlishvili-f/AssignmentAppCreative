using AssignmentAppCreative.Controllers.WeatherServiceRelated;
using AssignmentAppCreative.HelperClasses;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace AssignmentAppCreative.Controllers;

public interface IWeatherService
{
    public Task<T?> GetWeatherData<T>(RestRequest request);

    // string GetWeatherDataString(RestRequest request);
}

[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    readonly IWeatherService _weatherService;
    readonly ICacheManager _cacheManager;

    public WeatherController(IWeatherService weatherService, ICacheManager cacheManager)
    {
        _weatherService = weatherService;
        _cacheManager = cacheManager;
    }

    [HttpGet("{cityName}")]
    // [EnableCors(Value)]
    public async Task<string?> GetWeatherForCity(string cityName)
    {
        var valueFromCache = await _cacheManager.GetValueForAsync(cityName);

        if (valueFromCache.IsNotNullOrEmpty()) return valueFromCache;

        var coordinatesResponse = await _weatherService.GetWeatherData<IEnumerable<Root>>(
            new RestRequest("geo/1.0/direct")
                .AddParameter("q", cityName)
                .AddParameter("limit", "5"));

        if (coordinatesResponse == null) return "Nothing was returned";

        var returnValue = (await coordinatesResponse.Select(async c =>
            {
                var response = await _weatherService.GetWeatherData<Current>(new RestRequest("data/2.5/weather")
                    .AddParameter("lat", c.Lat)
                    .AddParameter("lon", c.Lon)
                    .AddParameter("units", "metric")
                    .AddParameter("mode", "xml"));

                return response?.ToString() ?? "nothing";
            }).WhenAll())
            .FirstOrDefault();

        await _cacheManager.SetValueFor(cityName, returnValue);
        return returnValue;
    }
}

public enum SpeedUnitEnum
{
    MetersPerSecond = 0,
    KilometersPerHour = 1
}