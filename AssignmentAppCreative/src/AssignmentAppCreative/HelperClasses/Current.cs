using System.Xml.Serialization;

namespace AssignmentAppCreative.HelperClasses;

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

Wind: {Wind?.Speed?.ToString() ?? Constants.Missing}
Wind direction: {Wind?.Direction?.ToString() ?? Constants.Missing}
Pressure: {Pressure?.Value ?? Constants.Missing}
Humidity: {Humidity?.Value ?? Constants.Missing}";
}