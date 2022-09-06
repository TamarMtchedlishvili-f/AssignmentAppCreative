using System.Collections.Immutable;
using System.Globalization;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using TM_LambdaASP.NETCoreWebAPI.Controllers.WeatherServiceRelated;
using static TM_LambdaASP.NETCoreWebAPI.Controllers.Constants;

namespace TM_LambdaASP.NETCoreWebAPI.Controllers;

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

[XmlRoot(ElementName = Constants.Temperature)]
public class Temperature
{
    [XmlAttribute(AttributeName = Constants.Value)]
    public string? Value { get; init; }

    [XmlAttribute(AttributeName = Constants.Unit)] public string? Unit { get; init; }

    public override string ToString()
        => $"{Value} " +
           Unit switch
           {
               "celsius" => "°C",
               _ => Unit
           };
}

[XmlRoot(ElementName = "feels_like")]
public class FeelsLike
{
    [XmlAttribute(AttributeName = "value")]
    public string? Value { get; init; }

    [XmlAttribute(AttributeName = Constants.Unit)] public string? Unit { get; init; }
}

[XmlRoot(ElementName = Constants.Humidity)]
public class Humidity
{
    [XmlAttribute(AttributeName = Constants.Value)]
    public string? Value { get; init; }

    [XmlAttribute(AttributeName = Constants.Unit)] public string? Unit { get; init; }
}

[XmlRoot(ElementName = Constants.Pressure)]
public class Pressure
{
    [XmlAttribute(AttributeName = Constants.Value)]
    public string? Value { get; init; }

    [XmlAttribute(AttributeName = Constants.Unit)]
    public string? Unit { get; init; }
}

[XmlRoot(ElementName = Constants.Speed)]
public class Speed
{
    [XmlAttribute(AttributeName = Constants.Value)]
    public double Value { get; init; }

    [XmlAttribute(AttributeName = Constants.Unit)] public string? Unit { get; init; }
    [XmlAttribute(AttributeName = Constants.Name)] public string? Name { get; init; }

    public SpeedValueObject? SpeedV =>
        Unit != null
            ? SpeedValueObject.From(Unit, Value).ToKilometersPerHour()
            : null;

    public override string ToString() => SpeedV?.ToString() ?? Missing;
}

[XmlRoot(ElementName = Constants.Direction)]
public class Direction
{
    [XmlAttribute(AttributeName = Constants.Value)]
    public string? Value { get; init; }

    [XmlAttribute(AttributeName = "code")] public string? Code { get; init; }
    [XmlAttribute(AttributeName = Constants.Name)] public string? Name { get; init; }

    public override string ToString() => $"{Code} ({Name})";
}

[XmlRoot(ElementName = Constants.Wind)]
public class Wind
{
    [XmlElement(ElementName = Constants.Speed)] public Speed? Speed { get; init; }

    [XmlElement(ElementName = Constants.Direction)]
    public Direction? Direction { get; init; }
}

[XmlRoot(ElementName = Constants.Weather)]
public class Weather
{
    [XmlAttribute(AttributeName = Constants.Value)]
    public string? Value { get; init; }

    public override string ToString() => Value?.ToTitleCase() ?? Missing;
}

public class Constants
{
    public const string Missing = nameof(Missing);
    public const string Value = "value";
    public const string Unit = "unit";
    public const string Name = "name";
    public const string Speed = "speed";
    public const string Wind = "wind";
    public const string Pressure = "pressure";
    public const string Humidity = "humidity";
    public const string Weather = "weather";
    public const string Direction = "direction";
    public const string Temperature = "temperature";
    
}

[XmlRoot(ElementName = "current")]
public class Current
{
    [XmlElement(ElementName = Constants.Temperature)]
    public Temperature? Temperature { get; init; }

    [XmlElement(ElementName = Constants.Humidity)] public Humidity? Humidity { get; init; }
    [XmlElement(ElementName = Constants.Pressure)] public Pressure? Pressure { get; init; }
    [XmlElement(ElementName = Constants.Wind)] public Wind? Wind { get; init; }
    [XmlElement(ElementName = Constants.Weather)] public Weather? Weather { get; init; }

    public override string ToString()
        => $@"Temperature: {Temperature} 
Weather conditions: {Weather}

Wind: {Wind?.Speed?.ToString() ?? Missing}
Wind direction: {Wind?.Direction?.ToString() ?? Missing}
Pressure: {Pressure?.Value ?? Missing}
Humidity: {Humidity?.Value ?? Missing}";
}

public class Root
{
    public string? Name { get; init; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public double Lat { get; init; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public double Lon { get; init; }
    // public string? Country { get; init; }
}

public class SpeedValueObject
{
    SpeedValueObject(string unit, double value)
        : this(
            Map.ContainsKey(unit)
                ? Map.GetValueOrDefault(unit)
                : throw new InvalidOperationException()
            , value)
    {
    }

    static readonly Dictionary<string, SpeedUnitEnum> Map = new()
    {
        { "m/s", SpeedUnitEnum.MetersPerSecond },
        { "km/h", SpeedUnitEnum.KilometersPerHour }
    };

    SpeedValueObject(SpeedUnitEnum unit, double value)
    {
        Value = value;
        Unit = unit;
        UnitString = Map.Single(m => m.Value == unit).Key;
    }

    public string UnitString { get; }
    public static SpeedValueObject From(string unit, double value) => new SpeedValueObject(unit, value);

    public SpeedUnitEnum Unit { get; }
    public double Value { get; }

    public SpeedValueObject ToKilometersPerHour()
        => Unit switch
        {
            SpeedUnitEnum.KilometersPerHour => this,
            SpeedUnitEnum.MetersPerSecond => new SpeedValueObject(SpeedUnitEnum.KilometersPerHour, Value * 3.6),
            _ => throw new InvalidOperationException("Can't be here")
        };

    public override string ToString() => $"{Value:F1} {UnitString}";
}

public enum SpeedUnitEnum
{
    MetersPerSecond = 0,
    KilometersPerHour = 1
}

public static class RestRequestExtensions
{
    public static bool IsNullOrEmpty(this string? text) => string.IsNullOrEmpty(text);
    public static bool IsNotNullOrEmpty(this string? text) => !text.IsNullOrEmpty();

    public static string ToTitleCase(this string title)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower());
    }

    // ReSharper disable once InconsistentNaming
    public static RestRequest AddAPIKey(this RestRequest request)
        => request.AddParameter("appid", "750047cf32f3161bd1d50cbb8646cf49");

    public static async Task<IEnumerable<T>> WhenAll<T>(this IEnumerable<Task<T>> items)
    {
        var itemsList = items.ToImmutableList();
        await Task.WhenAll(itemsList);

        return itemsList.Select(i => i.Result);
    }
}