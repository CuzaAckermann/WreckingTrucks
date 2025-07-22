using System;

public class GameButton : BaseButton
{
    public event Action Pressed;

    protected override void OnPressed()
    {
        Pressed?.Invoke();
    }
}