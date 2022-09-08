using System.Xml.Serialization;

namespace AssignmentAppCreative.DTOs;

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