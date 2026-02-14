public class InputStateSwitcher
{
    private readonly EventBus _eventBus;
    private readonly UpdateApplicationState _updateApplicationState;
    private readonly IWindowsStorage _windowsStorage;

    private readonly StateStackMachine<InputState<IInput>> _inputStateMachine;

    private readonly InputStateStorage _stateStorage;

    public InputStateSwitcher(EventBus eventBus,
                              UpdateApplicationState updateApplicationState,
                              IWindowsStorage windowsStorage,
                              InputStateStorage stateStorage)
    {
        Validator.ValidateNotNull(eventBus, updateApplicationState, windowsStorage, stateStorage);

        _eventBus = eventBus;
        _updateApplicationState = updateApplicationState;
        _windowsStorage = windowsStorage;
        _stateStorage = stateStorage;

        _inputStateMachine = new StateStackMachine<InputState<IInput>>();

        SubscribeToButtons();

        _eventBus.Subscribe<ClearedSignal<ApplicationSignal>>(Clear);

        _eventBus.Subscribe<EnabledSignal<ApplicationSignal>>(Start);

        _eventBus.Subscribe<CreatedSignal<Level>>(FinishPlayingState, Priority.High);
        _eventBus.Subscribe<CreatedSignal<Level>>(PreparePlayingState, Priority.Low);

        _updateApplicationState.Updated += Update;
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
        _windowsStorage.GameSelectionWindow.LevelsButton.Pressed += SwitchToLevelSelectionInputState;
        _windowsStorage.GameSelectionWindow.ReturnButton.Pressed += ReturnPreviousState;

        _windowsStorage.LevelButtonsStorage.ReturnButton.Pressed += ReturnPreviousState;

        _windowsStorage.OptionsMenu.ReturnButton.Pressed += ReturnPreviousState;

        _windowsStorage.ShopWindow.ReturnButton.Pressed += ReturnPreviousState;

        _windowsStorage.PlayingWindow.PauseButton.Pressed += SwitchToPauseInputState;
        _windowsStorage.PlayingWindow.SwapAbilityButton.Pressed += SwitchToSwapAbilityInputState;

        _windowsStorage.SwapAbilityWindow.ReturnButton.Pressed += ReturnPreviousState;

        _windowsStorage.PauseMenu.MainMenuButton.Pressed += ResetStates;
        _windowsStorage.PauseMenu.ReturnButton.Pressed += ReturnPreviousState;

        //_windowsStorage.PauseMenu.ResetLevelButton.Pressed += ReturnPreviousState;
        //_windowsStorage.PauseMenu.ResetLevelButton.Pressed += ReturnPreviousState;

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
        _windowsStorage.GameSelectionWindow.LevelsButton.Pressed -= SwitchToLevelSelectionInputState;
        _windowsStorage.GameSelectionWindow.ReturnButton.Pressed -= ReturnPreviousState;

        _windowsStorage.LevelButtonsStorage.ReturnButton.Pressed -= ReturnPreviousState;

        _windowsStorage.OptionsMenu.ReturnButton.Pressed -= ReturnPreviousState;

        _windowsStorage.ShopWindow.ReturnButton.Pressed -= ReturnPreviousState;

        _windowsStorage.PlayingWindow.PauseButton.Pressed -= SwitchToPauseInputState;
        _windowsStorage.PlayingWindow.SwapAbilityButton.Pressed -= SwitchToSwapAbilityInputState;

        _windowsStorage.SwapAbilityWindow.ReturnButton.Pressed -= ReturnPreviousState;

        _windowsStorage.PauseMenu.MainMenuButton.Pressed -= ResetStates;
        _windowsStorage.PauseMenu.ReturnButton.Pressed -= ReturnPreviousState;

        //_windowsStorage.PauseMenu.ResetLevelButton.Pressed -= ReturnPreviousState;
        //_windowsStorage.PauseMenu.ResetLevelButton.Pressed -= ReturnPreviousState;

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

    // днонкмхрэ, онунфе нм асдер осакхвмшл
    private void ReturnPreviousState()
    {
        _inputStateMachine.PopState();
    }

    private void Update()
    {
        _inputStateMachine.Update();
    }

    private void Clear(ClearedSignal<ApplicationSignal> _)
    {
        _eventBus.Unsubscribe<ClearedSignal<ApplicationSignal>>(Clear);

        _eventBus.Unsubscribe<EnabledSignal<ApplicationSignal>>(Start);

        _eventBus.Unsubscribe<CreatedSignal<Level>>(FinishPlayingState);
        _eventBus.Unsubscribe<CreatedSignal<Level>>(PreparePlayingState);

        _updateApplicationState.Updated += Update;

        UnsubscribeFromButtons();
    }

    private void Start(EnabledSignal<ApplicationSignal> _)
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