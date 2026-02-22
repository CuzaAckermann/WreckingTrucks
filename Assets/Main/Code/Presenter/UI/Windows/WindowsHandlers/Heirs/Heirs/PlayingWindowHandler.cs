public class PlayingWindowHandler : WindowHandler<PlayingWindow>
{
    public PlayingWindowHandler(PlayingWindow stateWindow, InputStateStorage storage) : base(stateWindow, storage)
    {

    }

    protected override void SubscribeToWindow(PlayingWindow window)
    {
        window.PauseButton.Pressed += OnInputStateStarting<PausedInputState>;
        window.SwapAbilityButton.Pressed += OnInputStateStarting<SwapAbilityInputState>;
    }

    protected override void UnsubscribeFromWindow(PlayingWindow window)
    {
        window.PauseButton.Pressed -= OnInputStateStarting<PausedInputState>;
        window.SwapAbilityButton.Pressed -= OnInputStateStarting<SwapAbilityInputState>;
    }
}