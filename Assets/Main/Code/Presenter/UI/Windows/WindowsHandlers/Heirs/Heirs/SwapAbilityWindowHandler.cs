public class SwapAbilityWindowHandler : WindowHandler<SwapAbilityWindow>
{
    public SwapAbilityWindowHandler(SwapAbilityWindow stateWindow, InputStateStorage storage) : base(stateWindow, storage)
    {

    }

    protected override void SubscribeToWindow(SwapAbilityWindow window)
    {
        window.ReturnButton.Pressed += OnInputStateReturning;
    }

    protected override void UnsubscribeFromWindow(SwapAbilityWindow window)
    {
        window.ReturnButton.Pressed -= OnInputStateReturning;
    }
}