public class ComputerGameplayWindowHandler : WindowHandler<ComputerGameplayWindow>
{
    public ComputerGameplayWindowHandler(ComputerGameplayWindow stateWindow, InputStateStorage storage) : base(stateWindow, storage)
    {

    }

    protected override void SubscribeToWindow(ComputerGameplayWindow window)
    {
        window.ReturnButton.Pressed += OnInputStateReturning;
    }

    protected override void UnsubscribeFromWindow(ComputerGameplayWindow window)
    {
        window.ReturnButton.Pressed -= OnInputStateReturning;
    }
}