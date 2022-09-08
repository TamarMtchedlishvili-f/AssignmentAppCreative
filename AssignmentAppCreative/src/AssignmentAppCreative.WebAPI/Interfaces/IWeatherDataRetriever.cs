namespace AssignmentAppCreative.Interfaces;

public interface IWeatherDataRetriever
{
    Task<string?> GetWeatherForFirstCity(string cityName);
}