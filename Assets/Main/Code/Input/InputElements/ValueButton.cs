using UnityEngine;

public class ValueButton : DeviceButton
{
    private readonly float _value;

    public ValueButton(KeyCode button, IPressMode pressMode, float value) : base(button, pressMode)
    {
        _value = value;
    }

    public float Value => _value;
}