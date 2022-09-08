using System.Xml.Serialization;

namespace AssignmentAppCreative.WebAPI.DTOs;

[XmlRoot(ElementName = Constants.Direction)]
public class Direction
{
    [XmlAttribute(AttributeName = Constants.Value)]
    public string? Value { get; init; }

    [XmlAttribute(AttributeName = "code")] public string? Code { get; init; }
    [XmlAttribute(AttributeName = Constants.Name)] public string? Name { get; init; }

    public override string ToString() => $"{Code} ({Name})";
}