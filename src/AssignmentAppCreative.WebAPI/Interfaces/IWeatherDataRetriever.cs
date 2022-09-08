namespace AssignmentAppCreative.WebAPI.Interfaces;

public interface IWeatherDataRetriever
{
    Task<string?> GetWeatherForFirstCity(string cityName);
}