using System;

public class CartrigeBoxTypeConverter : ITypeConverter
{
    public Type GetModelType(ColorType colorType)
    {
        return typeof(CartrigeBox);
    }
}