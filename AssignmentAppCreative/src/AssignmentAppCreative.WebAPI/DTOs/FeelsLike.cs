using System.Xml.Serialization;

namespace AssignmentAppCreative.DTOs;

[XmlRoot(ElementName = "feels_like")]
public class FeelsLike
{
    [XmlAttribute(AttributeName = Constants.Value)]
    public string? Value { get; init; }

    [XmlAttribute(AttributeName = Constants.Unit)] public string? Unit { get; init; }
}