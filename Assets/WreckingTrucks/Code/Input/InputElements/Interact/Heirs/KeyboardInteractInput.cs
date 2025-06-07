using UnityEngine;

public class KeyboardInteractInput : IInteractInput
{
    private readonly KeyCode _key;
    private readonly bool _isHold;

    public KeyboardInteractInput(KeyCode key, bool isHold)
    {
        _key = key;
        _isHold = isHold;
    }

    public bool IsPressed()
    {
        return _isHold ? Input.GetKey(_key) : Input.GetKeyDown(_key);
    }
}