using UnityEngine;

public class KeyboardInput : IElementInput
{
    private readonly KeyCode _key;

    public KeyboardInput(KeyCode key)
    {
        _key = key;
    }

    public bool IsPressed()
    {
        return Input.GetKeyDown(_key);
    }
}