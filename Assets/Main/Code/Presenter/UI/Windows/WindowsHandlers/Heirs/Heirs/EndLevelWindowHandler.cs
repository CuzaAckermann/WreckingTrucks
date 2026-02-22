public class EndLevelWindowHandler : WindowHandler<EndLevelWindow>
{
    public EndLevelWindowHandler(EndLevelWindow stateWindow, InputStateStorage storage) : base(stateWindow, storage)
    {

    }

    protected override void SubscribeToWindow(EndLevelWindow window)
    {
        window.MainMenuButton.Pressed += OnInputStateStarting<MainMenuInputState>;
        window.LevelSelectionButton.Pressed += OnInputStateStarting<LevelSelectionInputState>;
    }

    protected override void UnsubscribeFromWindow(EndLevelWindow window)
    {
        window.MainMenuButton.Pressed -= OnInputStateStarting<MainMenuInputState>;
        window.LevelSelectionButton.Pressed -= OnInputStateStarting<LevelSelectionInputState>;
    }
}