using System;
using UnityEngine;

public class KeyboardButton : IInputButton
{
    private readonly KeyCode _key;
    private readonly IPressMode _pressMode;

    public KeyboardButton(KeyCode key, IPressMode pressMode)
    {
        Validator.ValidateNotNull(pressMode);

        _key = key;
        _pressMode = pressMode;
    }

    public event Action Pressed;

    public void Update()
    {
        if (_pressMode.IsPressed(_key))
        {
            Pressed?.Invoke();
        }
    }
}