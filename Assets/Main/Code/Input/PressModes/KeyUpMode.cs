using UnityEngine;

public class KeyUpMode : IPressMode
{
    public bool IsPressed(KeyCode keyCode)
    {
        return Input.GetKeyUp(keyCode);
    }
}