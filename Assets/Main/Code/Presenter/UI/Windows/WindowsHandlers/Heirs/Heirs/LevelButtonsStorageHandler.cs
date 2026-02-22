public class LevelButtonsStorageHandler : WindowHandler<LevelButtonsStorage>
{
    public LevelButtonsStorageHandler(LevelButtonsStorage stateWindow, InputStateStorage storage) : base(stateWindow, storage)
    {

    }

    protected override void SubscribeToWindow(LevelButtonsStorage window)
    {
        window.ReturnButton.Pressed += OnInputStateReturning;
    }

    protected override void UnsubscribeFromWindow(LevelButtonsStorage window)
    {
        window.ReturnButton.Pressed -= OnInputStateReturning;
    }
}