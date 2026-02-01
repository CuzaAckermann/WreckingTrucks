using System;

public class InputStateSwitcher
{
    private readonly EventBus _eventBus;
    private readonly IWindowSwitchingInformer _informer;

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
                              IWindowSwitchingInformer informer,
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
        _informer = informer ?? throw new ArgumentNullException(nameof(informer));

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

        SubscribeToInformer();

        _eventBus.Subscribe<ClearedSignal<GameSignalEmitter>>(Clear);

        _eventBus.Subscribe<EnabledSignal<GameSignalEmitter>>(Start);

        _eventBus.Subscribe<CreatedSignal<Level>>(FinishPlayingState, Priority.High);
        _eventBus.Subscribe<CreatedSignal<Level>>(PreparePlayingState, Priority.Low);

        _eventBus.Subscribe<UpdateSignal>(Update);
    }

    private void SubscribeToInformer()
    {
        _informer.HideMainMenuActivated += SwitchToBackgroundGameInputState;
        _informer.MainMenuActivated += Reset;
        _informer.OptionsMenuActivated += SwitchToOptionMenuInputState;
        _informer.ShopMenuActivated += SwitchToShopInputState;
        _informer.PlayingMenuActivated += SwitchToLevelSelectionInputState;
        _informer.SwapAbilityMenuActivated += SwitchToSwapAbilityInputState;
        _informer.PauseMenuActivated += SwitchToPauseInputState;
        _informer.ReturnActivated += Return;
    }

    private void UnsubscribeFromInformer()
    {
        _informer.HideMainMenuActivated -= SwitchToBackgroundGameInputState;
        _informer.MainMenuActivated -= Reset;
        _informer.OptionsMenuActivated -= SwitchToOptionMenuInputState;
        _informer.ShopMenuActivated -= SwitchToShopInputState;
        _informer.PlayingMenuActivated -= SwitchToLevelSelectionInputState;
        _informer.SwapAbilityMenuActivated -= SwitchToSwapAbilityInputState;
        _informer.PauseMenuActivated -= SwitchToPauseInputState;
        _informer.ReturnActivated -= Return;
    }

    private void SwitchToBackgroundGameInputState()
    {
        _inputStateMachine.PushState(_backgroundGameState);
    }

    private void Reset()
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

    private void Return()
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

        UnsubscribeFromInformer();
    }

    private void Start(EnabledSignal<GameSignalEmitter> _)
    {
        Reset();
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