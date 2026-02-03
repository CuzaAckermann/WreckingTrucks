using System;

public class InputStateSwitcher
{
    private readonly EventBus _eventBus;
    private readonly IWindowsStorage _windowsStorage;

    private readonly InputStateMachine _inputStateMachine;

    private readonly StateStorage _stateStorage;

    public InputStateSwitcher(EventBus eventBus,
                              IWindowsStorage windowsStorage,
                              StateStorage stateStorage)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _windowsStorage = windowsStorage ?? throw new ArgumentNullException(nameof(windowsStorage));
        _stateStorage = stateStorage ?? throw new ArgumentNullException(nameof(stateStorage));

        _inputStateMachine = new InputStateMachine();

        SubscribeToButtons();

        _eventBus.Subscribe<ClearedSignal<GameSignalEmitter>>(Clear);

        _eventBus.Subscribe<EnabledSignal<GameSignalEmitter>>(Start);

        _eventBus.Subscribe<CreatedSignal<Level>>(FinishPlayingState, Priority.High);
        _eventBus.Subscribe<CreatedSignal<Level>>(PreparePlayingState, Priority.Low);

        _eventBus.Subscribe<UpdateSignal>(Update);
    }

    private void SubscribeToButtons()
    {
        _windowsStorage.BackgroundGameWindow.ReturnButton.Pressed += ReturnPreviousState;

        _windowsStorage.MainMenuWindow.HideMenuButton.Pressed += SwitchToBackgroundGameInputState;
        _windowsStorage.MainMenuWindow.PlayButton.Pressed += SwitchToGameSelectionState;
        _windowsStorage.MainMenuWindow.OptionsButton.Pressed += SwitchToOptionMenuInputState;
        _windowsStorage.MainMenuWindow.ShopButton.Pressed += SwitchToShopInputState;

        //_windowsStorage.GameSelectionWindow.StartNewGameButton.Pressed += ;
        //_windowsStorage.GameSelectionWindow.ContinueButton.Pressed += ;
        _windowsStorage.GameSelectionWindow.LevelSelectionButton.Pressed += SwitchToLevelSelectionInputState;
        _windowsStorage.GameSelectionWindow.ReturnButton.Pressed += ReturnPreviousState;

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

    private void UnsubscribeFromButtons()
    {
        _windowsStorage.BackgroundGameWindow.ReturnButton.Pressed -= ReturnPreviousState;

        _windowsStorage.MainMenuWindow.HideMenuButton.Pressed -= SwitchToBackgroundGameInputState;
        _windowsStorage.MainMenuWindow.PlayButton.Pressed -= SwitchToGameSelectionState;
        _windowsStorage.MainMenuWindow.OptionsButton.Pressed -= SwitchToOptionMenuInputState;
        _windowsStorage.MainMenuWindow.ShopButton.Pressed -= SwitchToShopInputState;

        //_windowsStorage.GameSelectionWindow.StartNewGameButton.Pressed -= ;
        //_windowsStorage.GameSelectionWindow.ContinueButton.Pressed -= ;
        _windowsStorage.GameSelectionWindow.LevelSelectionButton.Pressed -= SwitchToLevelSelectionInputState;
        _windowsStorage.GameSelectionWindow.ReturnButton.Pressed -= ReturnPreviousState;

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
        _inputStateMachine.PushState(_stateStorage.BackgroundGameState);
    }

    private void ResetStates()
    {
        _inputStateMachine.ClearStates();
        _inputStateMachine.PushState(_stateStorage.MainMenuState);
    }

    private void SwitchToOptionMenuInputState()
    {
        _inputStateMachine.PushState(_stateStorage.OptionsMenuState);
    }

    private void SwitchToShopInputState()
    {
        _inputStateMachine.PushState(_stateStorage.ShopState);
    }

    private void SwitchToGameSelectionState()
    {
        _inputStateMachine.PushState(_stateStorage.GameSelectionState);
    }

    private void SwitchToLevelSelectionInputState()
    {
        _inputStateMachine.PushState(_stateStorage.LevelSelectionState);
    }

    private void SwitchToSwapAbilityInputState()
    {
        _inputStateMachine.PushState(_stateStorage.SwapAbilityState);
    }

    private void SwitchToPauseInputState()
    {
        _inputStateMachine.PushState(_stateStorage.PausedState);
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

        UnsubscribeFromButtons();
    }

    private void Start(EnabledSignal<GameSignalEmitter> _)
    {
        ResetStates();
    }

    private void PreparePlayingState(CreatedSignal<Level> _)
    {
        _eventBus.Subscribe<CompletedSignal<Level>>(OnLevelPassed);
        _eventBus.Subscribe<FailedSignal<Level>>(OnLevelFailed);

        _inputStateMachine.PushState(_stateStorage.PlayingState);
    }

    private void FinishPlayingState(CreatedSignal<Level> _)
    {
        _eventBus.Unsubscribe<CompletedSignal<Level>>(OnLevelPassed);
        _eventBus.Unsubscribe<FailedSignal<Level>>(OnLevelFailed);
    }

    private void OnLevelPassed(CompletedSignal<Level> _)
    {
        _inputStateMachine.PushState(_stateStorage.EndLevelState);
    }

    private void OnLevelFailed(FailedSignal<Level> _)
    {
        _inputStateMachine.PushState(_stateStorage.EndLevelState);
    }
}