using System;

public class SpaceSettings
{
    private readonly FillingCard<Type> _fillingCard;

    public SpaceSettings(FillingCard<Type> fillingCard)
    {
        _fillingCard = fillingCard ?? throw new ArgumentNullException(nameof(fillingCard));
    }

    public int WidthField => _fillingCard.Width;

    public int LengthField => _fillingCard.Length;

    public FillingCard<Type> FillingCard => _fillingCard;
}