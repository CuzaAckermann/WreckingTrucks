using System;

public class SwapAbility
{
    private Field _field;

    public void SetField(Field field)
    {
        _field = field ?? throw new ArgumentNullException(nameof(field));
    }
}