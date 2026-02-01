using UnityEngine;

public class KeyboardContinuousInput : IElementInput
{
    private readonly KeyCode _key;
    private readonly bool _isHold;

    public KeyboardContinuousInput(KeyCode key, bool isHold)
    {
        _key = key;
        _isHold = isHold;
    }

    public bool IsPressed()
    {
        return _isHold ? Input.GetKeyDown(_key) : Input.GetKey(_key);
    }
}