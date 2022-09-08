using AssignmentAppCreative.WebAPI.Interfaces;
using RestSharp;

namespace AssignmentAppCreative.WebAPI.Services;

public class WeatherService : IWeatherService
{
    readonly IAwsSecretManager _secretManager;

    public WeatherService(IAwsSecretManager secretManager)
    {
        _secretManager = secretManager;
    }

    public async Task<T?> GetWeatherData<T>(RestRequest request)
    {
        var secretKey = await _secretManager.GetSecret("APIkey_AssignmentAppCreativeTM__Value");
        using var client = new RestClient("http://api.openweathermap.org");
        var coordinatesRequest = request
            .AddParameter("appid", secretKey);

        var coordinatesResponse = (await client.ExecuteAsync<T>(coordinatesRequest)).Data;

        return coordinatesResponse ?? default;
    }
}