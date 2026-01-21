using System;

public class Game
{
    private readonly EventBus _eventBus;

    private readonly LevelCreator _levelCreator;
    private readonly TickEngine _tickEngine;
    private readonly GameStateMachine _gameStateMachine;

    private readonly BackgroundGameCreator _backgroundGameCreator;

    private readonly BackgroundGameState _backgroundGameState;
    private readonly MainMenuState _mainMenuState;
    private readonly LevelSelectionState _levelSelectionState;
    private readonly OptionsMenuState _optionsMenuState;
    private readonly ShopState _shopState;
    private readonly PlayingState _playingState;
    private readonly SwapAbilityState _swapAbilityState;
    private readonly PausedState _pausedState;
    private readonly EndLevelState _endLevelState;

    //private BackgroundGame _backgroundGame;

    public Game(EventBus eventBus,
                LevelCreator gameWorldCreator,
                TickEngine tickEngine,
                BackgroundGameCreator backgroundGameCreator,
                BackgroundGameState backgroundGameState,
                MainMenuState mainMenuState,
                LevelSelectionState levelSelectionState,
                OptionsMenuState optionsMenuState,
                ShopState shopState,
                PlayingState playingState,
                SwapAbilityState swapAbilityState,
                PausedState pausedState,
                EndLevelState endLevelState)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        _levelCreator = gameWorldCreator ?? throw new ArgumentNullException(nameof(gameWorldCreator));
        _tickEngine = tickEngine ?? throw new ArgumentNullException(nameof(tickEngine));
        _gameStateMachine = new GameStateMachine();

        _backgroundGameCreator = backgroundGameCreator ?? throw new ArgumentNullException(nameof(backgroundGameCreator));

        _backgroundGameState = backgroundGameState ?? throw new ArgumentNullException(nameof(backgroundGameState));
        _mainMenuState = mainMenuState ?? throw new ArgumentNullException(nameof(mainMenuState));
        _levelSelectionState = levelSelectionState ?? throw new ArgumentNullException(nameof(levelSelectionState));
        _optionsMenuState = optionsMenuState ?? throw new ArgumentNullException(nameof(optionsMenuState));
        _shopState = shopState ?? throw new ArgumentNullException(nameof(shopState));
        _playingState = playingState ?? throw new ArgumentNullException(nameof(playingState));
        _swapAbilityState = swapAbilityState ?? throw new ArgumentNullException(nameof(swapAbilityState));
        _pausedState = pausedState ?? throw new ArgumentNullException(nameof(pausedState));
        _endLevelState = endLevelState ?? throw new ArgumentNullException(nameof(endLevelState));

        _eventBus.Subscribe<CreatedSignal<Level>>(FinishPlayingState, Priority.High);
        _eventBus.Subscribe<CreatedSignal<Level>>(PreparePlayingState, Priority.Low);
    }

    public bool HasNextLevel => _levelCreator.CanCreateNextGameWorld();

    public bool HasPreviousLevel => _levelCreator.CanCreatePreviousGameWorld();

    public void Clear()
    {
        _eventBus.Unsubscribe<CreatedSignal<Level>>(FinishPlayingState);
        _eventBus.Unsubscribe<CreatedSignal<Level>>(PreparePlayingState);

        _eventBus.Invoke(new ClearedSignal<Game>());
    }

    public void Start()
    {
        _eventBus.Invoke(new EnabledSignal<Game>());

        _tickEngine.Continue();
        OpenMainMenu();
    }

    public void Update(float deltaTime)
    {
        _gameStateMachine.Update(deltaTime);
        _tickEngine.Tick(deltaTime);
    }

    public void Stop()
    {
        _eventBus.Invoke(new DisabledSignal<Game>());
    }

    #region Windows handlers
    public void ShowBackgroundGame()
    {
        _gameStateMachine.PushState(_backgroundGameState);
    }

    public void OpenMainMenu()
    {
        // Пробовать начать с переделывания PlayingState, может пусть он делает всё сам?

        //FinishPlayingState();

        //_backgroundGame = _backgroundGameCreator.Create();
        //_backgroundGame.Prepare(_gameWorldCreator.CreateNonstopGame());
        //_backgroundGame.Enable();

        _gameStateMachine.ClearStates();
        _gameStateMachine.PushState(_mainMenuState);
    }

    public void ActivateLevelSelection()
    {
        _gameStateMachine.PushState(_levelSelectionState);
    }

    public void OpenOptions()
    {
        _gameStateMachine.PushState(_optionsMenuState);
    }

    public void OpenShop()
    {
        _gameStateMachine.PushState(_shopState);
    }

    public void BuildLevel(int indexOfLevel)
    {
        //FinishPlayingState();

        //
        _levelCreator.CreateLevelGame(indexOfLevel);
        //

        //PreparePlayingState();
    }

    public void ActivateNonstopGame()
    {
        //FinishPlayingState();

        //
        _levelCreator.CreateNonstopGame();
        //

        //PreparePlayingState();
    }

    public void PlayNextLevel()
    {
        //FinishPlayingState();

        //
        _levelCreator.CreateNextGameWorld();
        //

        //PreparePlayingState();
    }

    public void PlayPreviousLevel()
    {
        //FinishPlayingState();

        //
        _levelCreator.CreatePreviousGameWorld();
        //

        //PreparePlayingState();
    }

    public void Reset()
    {
        //FinishPlayingState();

        //
        _levelCreator.Recreate();
        //

        //PreparePlayingState();
    }

    public void Return()
    {
        _gameStateMachine.PopState();
    }

    public void Pause()
    {
        _gameStateMachine.PushState(_pausedState);
    }

    public void ActivateSwapAbility()
    {
        _gameStateMachine.PushState(_swapAbilityState);
    }
    #endregion

    private void PreparePlayingState(CreatedSignal<Level> _)
    {
        //_backgroundGame.Disable();
        //_backgroundGame.Clear();

        _eventBus.Subscribe<CompletedSignal<Level>>(OnLevelPassed);
        _eventBus.Subscribe<FailedSignal<Level>>(OnLevelFailed);

        _gameStateMachine.PushState(_playingState);
    }

    private void FinishPlayingState(CreatedSignal<Level> _)
    {
        _eventBus.Unsubscribe<CompletedSignal<Level>>(OnLevelPassed);
        _eventBus.Unsubscribe<FailedSignal<Level>>(OnLevelFailed);
    }

    private void OnLevelPassed(CompletedSignal<Level> _)
    {
        _gameStateMachine.PushState(_endLevelState);
    }

    private void OnLevelFailed(FailedSignal<Level> _)
    {
        _gameStateMachine.PushState(_endLevelState);
    }
}