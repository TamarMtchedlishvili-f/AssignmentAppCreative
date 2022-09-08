using AssignmentAppCreative.HelperClasses;
using RestSharp;
using StackExchange.Redis;

namespace AssignmentAppCreative.Controllers.WeatherServiceRelated;

public class WeatherService : IWeatherService
{
    public async Task<T?> GetWeatherData<T>(RestRequest request)
    {
        using var client = new RestClient("http://api.openweathermap.org");
        var coordinatesRequest = request
            .AddAPIKey();

        var coordinatesResponse = (await client.ExecuteAsync<T>(coordinatesRequest)).Data;

        return coordinatesResponse ?? default;
    }
}

public interface ICacheManager
{
    Task<string?> GetValueForAsync(string key);
    Task SetValueFor(string key, string? value);
}

public class CacheManager : ICacheManager
{
    ILogger<CacheManager> _logger;

    public CacheManager(ILogger<CacheManager> logger)
    {
        this._logger = logger;
    }

    public async Task<string?> GetValueForAsync(string key)
    {
        var db = Redis.GetDatabase();

        var returnValue = (await db.StringGetAsync(key)).ToString();

        _logger.LogInformation(returnValue.IsNullOrEmpty()
            ? $"Return value for [{key}] was null or empty"
            : $"Returned [{returnValue}] for key [{key}]");

        return returnValue;
    }

    public async Task SetValueFor(string key, string? value)
    {
        var db = Redis.GetDatabase();

        if ((await GetValueForAsync(key)).IsNotNullOrEmpty())
            throw new InvalidOperationException("Such value already exists");

        _logger.Log(LogLevel.Information, "Setting value");
        await db.StringSetAsync(key, value, TimeSpan.FromMinutes(1));
    }

    static readonly ConnectionMultiplexer Redis = ConnectionMultiplexer.Connect(
        new ConfigurationOptions
        {
            EndPoints = { "redis-16545.c300.eu-central-1-1.ec2.cloud.redislabs.com:16545" },
            Password = "S8TE3hv7WyRc9x3OHvvmyZqruVWwuATq"
        });
}