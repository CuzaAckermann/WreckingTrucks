public class GameSelectionWindowHandler : WindowHandler<GameSelectionWindow>
{
    public GameSelectionWindowHandler(GameSelectionWindow stateWindow, InputStateStorage storage) : base(stateWindow, storage)
    {

    }

    protected override void SubscribeToWindow(GameSelectionWindow window)
    {
        //window.StartNewGameButton.Pressed += ;
        //window.ContinueButton.Pressed += ;
        window.LevelsButton.Pressed += OnInputStateStarting<LevelSelectionInputState>;
        window.ReturnButton.Pressed += OnInputStateReturning;
    }

    protected override void UnsubscribeFromWindow(GameSelectionWindow window)
    {
        //window.StartNewGameButton.Pressed -= ;
        //window.ContinueButton.Pressed -= ;
        window.LevelsButton.Pressed -= OnInputStateStarting<LevelSelectionInputState>;
        window.ReturnButton.Pressed -= OnInputStateReturning;
    }
}