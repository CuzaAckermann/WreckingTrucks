public class PauseMenuHandler : WindowHandler<PauseMenu>
{
    public PauseMenuHandler(PauseMenu stateWindow, InputStateStorage storage) : base(stateWindow, storage)
    {

    }

    protected override void SubscribeToWindow(PauseMenu window)
    {
        window.MainMenuButton.Pressed += OnInputStateStarting<MainMenuInputState>;
        window.ReturnButton.Pressed += OnInputStateReturning;
        //window.ResetLevelButton.Pressed += ReturnPreviousState;
        //window.ResetLevelButton.Pressed += ReturnPreviousState;
        window.LevelSelectionButton.Pressed += OnInputStateStarting<LevelSelectionInputState>;
    }

    protected override void UnsubscribeFromWindow(PauseMenu window)
    {
        window.MainMenuButton.Pressed -= OnInputStateStarting<MainMenuInputState>;
        window.ReturnButton.Pressed -= OnInputStateReturning;
        //window.ResetLevelButton.Pressed -= ReturnPreviousState;
        //window.ResetLevelButton.Pressed -= ReturnPreviousState;
        window.LevelSelectionButton.Pressed -= OnInputStateStarting<LevelSelectionInputState>;
    }
}