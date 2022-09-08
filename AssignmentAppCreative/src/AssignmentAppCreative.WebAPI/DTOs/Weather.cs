using System.Xml.Serialization;
using AssignmentAppCreative.Extensions;

namespace AssignmentAppCreative.DTOs;

[XmlRoot(ElementName = Constants.Weather)]
public class Weather
{
    [XmlAttribute(AttributeName = Constants.Value)]
    public string? Value { get; init; }

    public override string ToString() => Value?.ToTitleCase() ?? Constants.Missing;
}