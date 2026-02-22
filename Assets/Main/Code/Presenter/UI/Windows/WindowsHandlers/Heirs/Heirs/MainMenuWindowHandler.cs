public class MainMenuWindowHandler : WindowHandler<MainMenuWindow>
{
    public MainMenuWindowHandler(MainMenuWindow stateWindow, InputStateStorage storage) : base(stateWindow, storage)
    {

    }

    protected override void SubscribeToWindow(MainMenuWindow window)
    {
        window.HideMenuButton.Pressed += OnInputStateStarting<ComputerGameplayInputState>;
        window.PlayButton.Pressed += OnInputStateStarting<GameSelectionInputState>;
        window.OptionsButton.Pressed += OnInputStateStarting<OptionsMenuInputState>;
        window.ShopButton.Pressed += OnInputStateStarting<ShopInputState>;
    }

    protected override void UnsubscribeFromWindow(MainMenuWindow window)
    {
        window.HideMenuButton.Pressed -= OnInputStateStarting<ComputerGameplayInputState>;
        window.PlayButton.Pressed -= OnInputStateStarting<GameSelectionInputState>;
        window.OptionsButton.Pressed -= OnInputStateStarting<OptionsMenuInputState>;
        window.ShopButton.Pressed -= OnInputStateStarting<ShopInputState>;
    }
}