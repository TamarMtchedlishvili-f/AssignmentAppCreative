using AssignmentAppCreative.WebAPI.DTOs;
using AssignmentAppCreative.WebAPI.Extensions;
using AssignmentAppCreative.WebAPI.Interfaces;
using RestSharp;

namespace AssignmentAppCreative.WebAPI.Services;

public class WeatherDataRetriever : IWeatherDataRetriever
{
    readonly IWeatherService _weatherService;
    readonly ICacheManager _cacheManager;

    public WeatherDataRetriever(IWeatherService weatherService, ICacheManager cacheManager)
    {
        _weatherService = weatherService;
        _cacheManager = cacheManager;
    }

    public async Task<string?> GetWeatherForFirstCity(string cityName)
    {
        var valueFromCache = await _cacheManager.GetValueForAsync(cityName);

        if (valueFromCache.IsNotNullOrEmpty()) return valueFromCache;

        var coordinatesResponse = await _weatherService.GetWeatherData<IEnumerable<LocationData>>(
            new RestRequest("geo/1.0/direct")
                .AddParameter("q", cityName)
                .AddParameter("limit", "5"));

        if (coordinatesResponse == null) return "Nothing was returned";

        var returnValue = (await coordinatesResponse.Select(async c =>
            {
                var response = await _weatherService.GetWeatherData<WeatherInfo>(
                    new RestRequest("data/2.5/weather")
                        .AddParameter("lat", c.Lat)
                        .AddParameter("lon", c.Lon)
                        .AddParameter("units", "metric")
                        .AddParameter("mode", "xml"));

                return response?.ToString() ?? "nothing";
            }).WhenAll())
            .FirstOrDefault();

        await _cacheManager.SetValueForAsync(cityName, returnValue);
        return returnValue;
    }
}