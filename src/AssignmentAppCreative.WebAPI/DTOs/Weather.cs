using System.Xml.Serialization;
using AssignmentAppCreative.WebAPI.Extensions;

namespace AssignmentAppCreative.WebAPI.DTOs;

[XmlRoot(ElementName = Constants.Weather)]
public class Weather
{
    [XmlAttribute(AttributeName = Constants.Value)]
    public string? Value { get; init; }

    public override string ToString() => Value?.ToTitleCase() ?? Constants.Missing;
}