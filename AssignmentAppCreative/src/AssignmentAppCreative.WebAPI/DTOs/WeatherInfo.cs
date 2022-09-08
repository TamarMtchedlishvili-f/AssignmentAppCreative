using System.Xml.Serialization;
using static AssignmentAppCreative.DTOs.Constants;

namespace AssignmentAppCreative.DTOs;

[XmlRoot(ElementName = "current")]
public class WeatherInfo
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