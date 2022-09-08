using System.Xml.Serialization;

namespace AssignmentAppCreative.HelperClasses;

[XmlRoot(ElementName = Constants.Wind)]
public class Wind
{
    [XmlElement(ElementName = Constants.Speed)] public Speed? Speed { get; init; }

    [XmlElement(ElementName = Constants.Direction)]
    public Direction? Direction { get; init; }
}