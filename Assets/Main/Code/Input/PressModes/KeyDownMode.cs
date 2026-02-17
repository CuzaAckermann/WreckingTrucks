using UnityEngine;

public class KeyDownMode : IPressMode
{
    public bool IsPressed(KeyCode keyCode)
    {
        return Input.GetKeyDown(keyCode);
    }
}