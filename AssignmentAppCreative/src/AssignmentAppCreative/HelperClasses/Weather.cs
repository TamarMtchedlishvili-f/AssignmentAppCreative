using System.Xml.Serialization;

namespace AssignmentAppCreative.HelperClasses;

[XmlRoot(ElementName = Constants.Weather)]
public class Weather
{
    [XmlAttribute(AttributeName = Constants.Value)]
    public string? Value { get; init; }

    public override string ToString() => Value?.ToTitleCase() ?? Constants.Missing;
}