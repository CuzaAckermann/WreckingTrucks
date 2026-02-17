public class InputStateSwitcher
{
    private readonly ApplicationStateStorage _applicationStateStorage;
    private readonly EventBus _eventBus;
    private readonly IWindowsStorage _windowsStorage;

    private readonly StateStackMachine<InputState<IInput>> _inputStateMachine;

    private readonly InputStateStorage _inputStateStorage;

    public InputStateSwitcher(ApplicationStateStorage applicationStateStorage,
                              EventBus eventBus,
                              IWindowsStorage windowsStorage,
                              InputStateStorage inputStateStorage)
    {
        Validator.ValidateNotNull(applicationStateStorage, eventBus, windowsStorage, inputStateStorage);

        _applicationStateStorage = applicationStateStorage;
        _eventBus = eventBus;
        _windowsStorage = windowsStorage;
        _inputStateStorage = inputStateStorage;

        _inputStateMachine = new StateStackMachine<InputState<IInput>>();

        SubscribeToButtons();

        _applicationStateStorage.FinishApplicationState.Triggered += Clear;

        _applicationStateStorage.PrepareApplicationState.Triggered += Start;

        _eventBus.Subscribe<CreatedSignal<Level>>(FinishPlayingState, Priority.High);
        _eventBus.Subscribe<CreatedSignal<Level>>(PreparePlayingState, Priority.Low);

        _applicationStateStorage.UpdateApplicationState.Updated += Update;
    }

    public StateStackMachine<InputState<IInput>> InputStateMachine => _inputStateMachine;

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
        _inputStateMachine.PushState(_inputStateStorage.ComputerGameplayInputState);
    }

    private void ResetStates()
    {
        _inputStateMachine.ClearStates();
        _inputStateMachine.PushState(_inputStateStorage.MainMenuInputState);
    }

    private void SwitchToOptionMenuInputState()
    {
        _inputStateMachine.PushState(_inputStateStorage.OptionsMenuInputState);
    }

    private void SwitchToShopInputState()
    {
        _inputStateMachine.PushState(_inputStateStorage.ShopInputState);
    }

    private void SwitchToGameSelectionState()
    {
        _inputStateMachine.PushState(_inputStateStorage.GameSelectionInputState);
    }

    private void SwitchToLevelSelectionInputState()
    {
        _inputStateMachine.PushState(_inputStateStorage.LevelSelectionInputState);
    }

    private void SwitchToSwapAbilityInputState()
    {
        _inputStateMachine.PushState(_inputStateStorage.SwapAbilityInputState);
    }

    private void SwitchToPauseInputState()
    {
        _inputStateMachine.PushState(_inputStateStorage.PausedInputState);
    }

    // днонкмхрэ, онунфе нм асдер осакхвмшл
    private void ReturnPreviousState()
    {
        _inputStateMachine.PopState();
    }

    private void Update()
    {
        _inputStateStorage.DeveloperInputState.Update();

        _inputStateMachine.Update();
    }

    private void Clear()
    {
        _applicationStateStorage.FinishApplicationState.Triggered -= Clear;

        _applicationStateStorage.PrepareApplicationState.Triggered -= Start;

        _eventBus.Unsubscribe<CreatedSignal<Level>>(FinishPlayingState);
        _eventBus.Unsubscribe<CreatedSignal<Level>>(PreparePlayingState);

        _applicationStateStorage.UpdateApplicationState.Updated -= Update;

        UnsubscribeFromButtons();
    }

    private void Start()
    {
        ResetStates();
    }

    private void PreparePlayingState(CreatedSignal<Level> _)
    {
        _eventBus.Subscribe<CompletedSignal<Level>>(OnLevelPassed);
        _eventBus.Subscribe<FailedSignal<Level>>(OnLevelFailed);

        _inputStateMachine.PushState(_inputStateStorage.PlayingInputState);
    }

    private void FinishPlayingState(CreatedSignal<Level> _)
    {
        _eventBus.Unsubscribe<CompletedSignal<Level>>(OnLevelPassed);
        _eventBus.Unsubscribe<FailedSignal<Level>>(OnLevelFailed);
    }

    private void OnLevelPassed(CompletedSignal<Level> _)
    {
        _inputStateMachine.PushState(_inputStateStorage.EndLevelInputState);
    }

    private void OnLevelFailed(FailedSignal<Level> _)
    {
        _inputStateMachine.PushState(_inputStateStorage.EndLevelInputState);
    }
}