using RestSharp;

namespace AssignmentAppCreative.WebAPI.Interfaces;

public interface IWeatherService
{
    public Task<T?> GetWeatherData<T>(RestRequest request);
}