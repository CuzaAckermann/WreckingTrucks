using System;

public class Colorable : IColorable
{
    public Colorable(ColorType color)
    {
        if (color == ColorType.Unknown)
        {
            throw new ArgumentException($"{nameof(color)} is {nameof(ColorType.Unknown)}");
        }

        Color = color;
    }

    public ColorType Color { get; private set; }
}