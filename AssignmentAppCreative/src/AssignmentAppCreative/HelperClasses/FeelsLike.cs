using System.Xml.Serialization;

namespace AssignmentAppCreative.HelperClasses;

[XmlRoot(ElementName = "feels_like")]
public class FeelsLike
{
    [XmlAttribute(AttributeName = "value")]
    public string? Value { get; init; }

    [XmlAttribute(AttributeName = Constants.Unit)] public string? Unit { get; init; }
}