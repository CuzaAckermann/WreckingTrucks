using UnityEngine;

public class KeyboardPauseInput : IPauseInput
{
    private readonly KeyCode _key;

    public KeyboardPauseInput(KeyCode key)
    {
        _key = key;
    }

    public bool IsPressed()
    {
        return Input.GetKeyDown(_key);
    }
}