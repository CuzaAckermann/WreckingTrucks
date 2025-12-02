using System;

[Serializable]
public class NonstopGameBlockFieldSettings : FieldSettings
{
    private ColorType[,,] _colorTypes;

    public void SetColorTypes(ColorType[,,] colorTypes)
    {
        _colorTypes = colorTypes ?? throw new ArgumentNullException(nameof(colorTypes));
    }

    public ColorType[,,] ColorTypes => _colorTypes;
}