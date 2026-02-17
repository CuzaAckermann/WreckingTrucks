using System;

public interface IInputButton
{
    public event Action Pressed;

    public void Update();
}