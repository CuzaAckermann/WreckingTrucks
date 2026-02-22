using System.Collections.Generic;

public class WindowHandlersCreator
{
    private readonly IWindowStorage _windowStorage;
    private readonly InputStateStorage _inputStateStorage;

    public WindowHandlersCreator(IWindowStorage windowStorage, InputStateStorage inputStateStorage)
    {
        Validator.ValidateNotNull(windowStorage, inputStateStorage);

        _windowStorage = windowStorage;
        _inputStateStorage = inputStateStorage;
    }

    public List<WindowHandlerBase> Create()
    {
        return new List<WindowHandlerBase>()
        {
            new ComputerGameplayWindowHandler(_windowStorage.Get<ComputerGameplayWindow>(), _inputStateStorage),
            new MainMenuWindowHandler(_windowStorage.Get<MainMenuWindow>(), _inputStateStorage),
            new GameSelectionWindowHandler(_windowStorage.Get<GameSelectionWindow>(), _inputStateStorage),
            new LevelButtonsStorageHandler(_windowStorage.Get<LevelButtonsStorage>(), _inputStateStorage),
            new OptionsMenuHandler(_windowStorage.Get<OptionsMenu>(), _inputStateStorage),
            new ShopWindowHandler(_windowStorage.Get<ShopWindow>(), _inputStateStorage),
            new PlayingWindowHandler(_windowStorage.Get<PlayingWindow>(), _inputStateStorage),
            new SwapAbilityWindowHandler(_windowStorage.Get<SwapAbilityWindow>(), _inputStateStorage),
            new PauseMenuHandler(_windowStorage.Get<PauseMenu>(), _inputStateStorage),
            new EndLevelWindowHandler(_windowStorage.Get<EndLevelWindow>(), _inputStateStorage)
        };
    }
}