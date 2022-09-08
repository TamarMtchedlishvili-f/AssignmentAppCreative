using System.Xml.Serialization;

namespace AssignmentAppCreative.HelperClasses;

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

    public override string ToString() => SpeedV?.ToString() ?? Constants.Missing;
}