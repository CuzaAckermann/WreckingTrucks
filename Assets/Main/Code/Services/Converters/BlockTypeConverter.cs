using System;

public class BlockTypeConverter : ITypeConverter
{
    public Type GetModelType(ColorType colorType)
    {
        return colorType switch
        {
            ColorType.Green => typeof(GreenBlock),
            ColorType.Orange => typeof(OrangeBlock),
            ColorType.Purple => typeof(PurpleBlock),
            _ => throw new ArgumentOutOfRangeException(nameof(colorType))
        };
    }
}