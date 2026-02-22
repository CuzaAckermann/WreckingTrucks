public class OptionsMenuHandler : WindowHandler<OptionsMenu>
{
    public OptionsMenuHandler(OptionsMenu stateWindow, InputStateStorage storage) : base(stateWindow, storage)
    {

    }

    protected override void SubscribeToWindow(OptionsMenu window)
    {
        window.ReturnButton.Pressed += OnInputStateReturning;
    }

    protected override void UnsubscribeFromWindow(OptionsMenu window)
    {
        window.ReturnButton.Pressed -= OnInputStateReturning;
    }
}