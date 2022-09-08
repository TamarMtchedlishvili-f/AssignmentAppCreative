using RestSharp;

namespace AssignmentAppCreative.Interfaces;

public interface IWeatherService
{
    public Task<T?> GetWeatherData<T>(RestRequest request);
}