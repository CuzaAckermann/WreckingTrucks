using System;

public class TruckTypeConverter : ITypeConverter
{
    public Type GetModelType(ColorType colorType)
    {
        return colorType switch
        {
            ColorType.Green => typeof(GreenTruck),
            ColorType.Orange => typeof(OrangeTruck),
            ColorType.Purple => typeof(PurpleTruck),
            _ => throw new ArgumentOutOfRangeException(nameof(colorType))
        };
    }
}