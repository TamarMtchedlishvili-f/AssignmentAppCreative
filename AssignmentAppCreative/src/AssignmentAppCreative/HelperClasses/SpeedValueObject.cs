using AssignmentAppCreative.Controllers;

namespace AssignmentAppCreative.HelperClasses;

public class SpeedValueObject
{
    SpeedValueObject(string unit, double value)
        : this(
            Map.ContainsKey(unit)
                ? Map.GetValueOrDefault(unit)
                : throw new InvalidOperationException()
            , value)
    {
    }

    static readonly Dictionary<string, SpeedUnitEnum> Map = new()
    {
        { "m/s", SpeedUnitEnum.MetersPerSecond },
        { "km/h", SpeedUnitEnum.KilometersPerHour }
    };

    SpeedValueObject(SpeedUnitEnum unit, double value)
    {
        Value = value;
        Unit = unit;
        UnitString = Map.Single(m => m.Value == unit).Key;
    }

    public string UnitString { get; }
    public static SpeedValueObject From(string unit, double value) => new SpeedValueObject(unit, value);

    public SpeedUnitEnum Unit { get; }
    public double Value { get; }

    public SpeedValueObject ToKilometersPerHour()
        => Unit switch
        {
            SpeedUnitEnum.KilometersPerHour => this,
            SpeedUnitEnum.MetersPerSecond => new SpeedValueObject(SpeedUnitEnum.KilometersPerHour, Value * 3.6),
            _ => throw new InvalidOperationException("Can't be here")
        };

    public override string ToString() => $"{Value:F1} {UnitString}";
}