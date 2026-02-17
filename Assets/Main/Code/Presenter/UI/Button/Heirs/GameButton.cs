using System;

public class GameButton : BaseUiButton
{
    public event Action Pressed;

    protected override void OnPressed()
    {
        Pressed?.Invoke();
    }
}