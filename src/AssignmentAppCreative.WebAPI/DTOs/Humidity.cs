using System.Xml.Serialization;

namespace AssignmentAppCreative.WebAPI.DTOs;

[XmlRoot(ElementName = Constants.Humidity)]
public class Humidity
{
    [XmlAttribute(AttributeName = Constants.Value)]
    public string? Value { get; init; }

    [XmlAttribute(AttributeName = Constants.Unit)] public string? Unit { get; init; }
}