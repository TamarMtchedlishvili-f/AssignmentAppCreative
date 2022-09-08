using System.Xml.Serialization;

namespace AssignmentAppCreative.WebAPI.DTOs;

[XmlRoot(ElementName = Constants.Wind)]
public class Wind
{
    [XmlElement(ElementName = Constants.Speed)] public Speed? Speed { get; init; }

    [XmlElement(ElementName = Constants.Direction)]
    public Direction? Direction { get; init; }
}