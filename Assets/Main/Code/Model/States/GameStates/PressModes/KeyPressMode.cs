using UnityEngine;

public class KeyPressMode : IPressMode
{
    public bool IsPressed(KeyCode keyCode)
    {
        return Input.GetKey(keyCode);
    }
}