using System;

public class Shifter
{
    private readonly Field _field;

    public Shifter(Field field)
    {
        _field = field ?? throw new ArgumentNullException(nameof(field));
    }

    public void ContinueShifting()
    {
        _field.ContinueShiftModels();
    }

    public void StopShifting()
    {
        _field.StopShiftModels();
    }
}