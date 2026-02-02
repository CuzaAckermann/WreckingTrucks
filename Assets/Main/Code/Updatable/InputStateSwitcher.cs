using System;

public class InputStateSwitcher
{
    private readonly EventBus _eventBus;
    private readonly IWindowsStorage _windowsStorage;

    private readonly InputStateMachine _inputStateMachine;

    private readonly BackgroundGameState _backgroundGameState;
    private readonly MainMenuInputState _mainMenuState;
    private readonly LevelSelectionState _levelSelectionState;
    private readonly OptionsMenuState _optionsMenuState;
    private readonly ShopState _shopState;
    private readonly PlayingInputState _playingState;
    private readonly SwapAbilityState _swapAbilityState;
    private readonly PausedState _pausedState;
    private readonly EndLevelState _endLevelState;

    public InputStateSwitcher(EventBus eventBus,
                              IWindowsStorage windowsStorage,
                              BackgroundGameState backgroundGameState,
                              MainMenuInputState mainMenuState,
                              LevelSelectionState levelSelectionState,
                              OptionsMenuState optionsMenuState,
                              ShopState shopState,
                              PlayingInputState playingState,
                              SwapAbilityState swapAbilityState,
                              PausedState pausedState,
                              EndLevelState endLevelState)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _windowsStorage = windowsStorage ?? throw new ArgumentNullException(nameof(windowsStorage));

        _inputStateMachine = new InputStateMachine();

        _backgroundGameState = backgroundGameState ?? throw new ArgumentNullException(nameof(backgroundGameState));
        _mainMenuState = mainMenuState ?? throw new ArgumentNullException(nameof(mainMenuState));
        _levelSelectionState = levelSelectionState ?? throw new ArgumentNullException(nameof(levelSelectionState));
        _optionsMenuState = optionsMenuState ?? throw new ArgumentNullException(nameof(optionsMenuState));
        _shopState = shopState ?? throw new ArgumentNullException(nameof(shopState));
        _playingState = playingState ?? throw new ArgumentNullException(nameof(playingState));
        _swapAbilityState = swapAbilityState ?? throw new ArgumentNullException(nameof(swapAbilityState));
        _pausedState = pausedState ?? throw new ArgumentNullException(nameof(pausedState));
        _endLevelState = endLevelState ?? throw new ArgumentNullException(nameof(endLevelState));

        SubscribeToWindows();

        _eventBus.Subscribe<ClearedSignal<GameSignalEmitter>>(Clear);

        _eventBus.Subscribe<EnabledSignal<GameSignalEmitter>>(Start);

        _eventBus.Subscribe<CreatedSignal<Level>>(FinishPlayingState, Priority.High);
        _eventBus.Subscribe<CreatedSignal<Level>>(PreparePlayingState, Priority.Low);

        _eventBus.Subscribe<UpdateSignal>(Update);
    }

    private void SubscribeToWindows()
    {
        _windowsStorage.BackgroundGameWindow.ReturnButton.Pressed += ReturnPreviousState;

        _windowsStorage.MainMenuWindow.HideMenuButton.Pressed += SwitchToBackgroundGameInputState;
        _windowsStorage.MainMenuWindow.PlayButton.Pressed += SwitchToLevelSelectionInputState;
        _windowsStorage.MainMenuWindow.OptionsButton.Pressed += SwitchToOptionMenuInputState;
        _windowsStorage.MainMenuWindow.ShopButton.Pressed += SwitchToShopInputState;

        _windowsStorage.LevelButtonsStorage.ReturnButton.Pressed += ReturnPreviousState;

        _windowsStorage.OptionsMenu.ReturnButton.Pressed += ReturnPreviousState;

        _windowsStorage.ShopWindow.ReturnButton.Pressed += ReturnPreviousState;

        _windowsStorage.PlayingWindow.PauseButton.Pressed += SwitchToPauseInputState;
        _windowsStorage.PlayingWindow.SwapAbilityButton.Pressed += SwitchToSwapAbilityInputState;

        _windowsStorage.SwapAbilityWindow.ReturnButton.Pressed += ReturnPreviousState;

        _windowsStorage.PauseMenu.MainMenuButton.Pressed += ResetStates;
        _windowsStorage.PauseMenu.ReturnButton.Pressed += ReturnPreviousState;
        _windowsStorage.PauseMenu.LevelSelectionButton.Pressed += SwitchToLevelSelectionInputState;

        _windowsStorage.EndLevelWindow.MainMenuButton.Pressed += ResetStates;
        _windowsStorage.EndLevelWindow.LevelSelectionButton.Pressed += SwitchToLevelSelectionInputState;
    }

    private void UnsubscribeFromWindows()
    {
        _windowsStorage.BackgroundGameWindow.ReturnButton.Pressed -= ReturnPreviousState;

        _windowsStorage.MainMenuWindow.HideMenuButton.Pressed -= SwitchToBackgroundGameInputState;
        _windowsStorage.MainMenuWindow.PlayButton.Pressed -= SwitchToLevelSelectionInputState;
        _windowsStorage.MainMenuWindow.OptionsButton.Pressed -= SwitchToOptionMenuInputState;
        _windowsStorage.MainMenuWindow.ShopButton.Pressed -= SwitchToShopInputState;

        _windowsStorage.LevelButtonsStorage.ReturnButton.Pressed -= ReturnPreviousState;

        _windowsStorage.OptionsMenu.ReturnButton.Pressed -= ReturnPreviousState;

        _windowsStorage.ShopWindow.ReturnButton.Pressed -= ReturnPreviousState;

        _windowsStorage.PlayingWindow.PauseButton.Pressed -= SwitchToPauseInputState;
        _windowsStorage.PlayingWindow.SwapAbilityButton.Pressed -= SwitchToSwapAbilityInputState;

        _windowsStorage.SwapAbilityWindow.ReturnButton.Pressed -= ReturnPreviousState;

        _windowsStorage.PauseMenu.MainMenuButton.Pressed -= ResetStates;
        _windowsStorage.PauseMenu.ReturnButton.Pressed -= ReturnPreviousState;
        _windowsStorage.PauseMenu.LevelSelectionButton.Pressed -= SwitchToLevelSelectionInputState;

        _windowsStorage.EndLevelWindow.MainMenuButton.Pressed -= ResetStates;
        _windowsStorage.EndLevelWindow.LevelSelectionButton.Pressed -= SwitchToLevelSelectionInputState;
    }

    private void SwitchToBackgroundGameInputState()
    {
        _inputStateMachine.PushState(_backgroundGameState);
    }

    private void ResetStates()
    {
        _inputStateMachine.ClearStates();
        _inputStateMachine.PushState(_mainMenuState);
    }

    private void SwitchToOptionMenuInputState()
    {
        _inputStateMachine.PushState(_optionsMenuState);
    }

    private void SwitchToShopInputState()
    {
        _inputStateMachine.PushState(_shopState);
    }

    private void SwitchToLevelSelectionInputState()
    {
        _inputStateMachine.PushState(_levelSelectionState);
    }

    private void SwitchToSwapAbilityInputState()
    {
        _inputStateMachine.PushState(_swapAbilityState);
    }

    private void SwitchToPauseInputState()
    {
        _inputStateMachine.PushState(_pausedState);
    }

    private void ReturnPreviousState()
    {
        _inputStateMachine.PopState();
    }

    private void Update(UpdateSignal _)
    {
        _inputStateMachine.Update();
    }

    private void Clear(ClearedSignal<GameSignalEmitter> _)
    {
        _eventBus.Unsubscribe<ClearedSignal<GameSignalEmitter>>(Clear);

        _eventBus.Unsubscribe<EnabledSignal<GameSignalEmitter>>(Start);

        _eventBus.Unsubscribe<CreatedSignal<Level>>(FinishPlayingState);
        _eventBus.Unsubscribe<CreatedSignal<Level>>(PreparePlayingState);

        _eventBus.Unsubscribe<UpdateSignal>(Update);

        UnsubscribeFromWindows();
    }

    private void Start(EnabledSignal<GameSignalEmitter> _)
    {
        ResetStates();
    }

    private void PreparePlayingState(CreatedSignal<Level> _)
    {
        _eventBus.Subscribe<CompletedSignal<Level>>(OnLevelPassed);
        _eventBus.Subscribe<FailedSignal<Level>>(OnLevelFailed);

        _inputStateMachine.PushState(_playingState);
    }

    private void FinishPlayingState(CreatedSignal<Level> _)
    {
        _eventBus.Unsubscribe<CompletedSignal<Level>>(OnLevelPassed);
        _eventBus.Unsubscribe<FailedSignal<Level>>(OnLevelFailed);
    }

    private void OnLevelPassed(CompletedSignal<Level> _)
    {
        _inputStateMachine.PushState(_endLevelState);
    }

    private void OnLevelFailed(FailedSignal<Level> _)
    {
        _inputStateMachine.PushState(_endLevelState);
    }
}