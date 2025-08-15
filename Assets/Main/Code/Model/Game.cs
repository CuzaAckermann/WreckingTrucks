using System;

public class Game
{
    private readonly GameWorldCreator _gameWorldCreator;
    private readonly TickEngine _tickEngine;
    private readonly GameStateMachine _gameStateMachine;

    private readonly BackgroundGameState _backgroundGameState;
    private readonly MainMenuState _mainMenuState;
    private readonly LevelSelectionState _levelSelectionState;
    private readonly OptionsMenuState _optionsMenuState;
    private readonly ShopState _shopState;
    private readonly PlayingState _playingState;
    private readonly SwapAbilityState _swapAbilityState;
    private readonly PausedState _pausedState;
    private readonly EndLevelState _endLevelState;

    public Game(GameWorldCreator gameWorldCreator,
                TickEngine tickEngine,
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
        _gameWorldCreator = gameWorldCreator ?? throw new ArgumentNullException(nameof(gameWorldCreator));
        _tickEngine = tickEngine ?? throw new ArgumentNullException(nameof(tickEngine));
        _gameStateMachine = new GameStateMachine();

        _backgroundGameState = backgroundGameState ?? throw new ArgumentNullException(nameof(backgroundGameState));
        _mainMenuState = mainMenuState ?? throw new ArgumentNullException(nameof(mainMenuState));
        _levelSelectionState = levelSelectionState ?? throw new ArgumentNullException(nameof(levelSelectionState));
        _optionsMenuState = optionsMenuState ?? throw new ArgumentNullException(nameof(optionsMenuState));
        _shopState = shopState ?? throw new ArgumentNullException(nameof(shopState));
        _playingState = playingState ?? throw new ArgumentNullException(nameof(playingState));
        _swapAbilityState = swapAbilityState ?? throw new ArgumentNullException(nameof(swapAbilityState));
        _pausedState = pausedState ?? throw new ArgumentNullException(nameof(pausedState));
        _endLevelState = endLevelState ?? throw new ArgumentNullException(nameof(endLevelState));
    }

    public event Action LevelPassed;
    public event Action LevelFailed;

    public event Action<GameWorld> GameWorldCreated;
    public event Action GameWorldDestroyed;

    public bool HasNextLevel => _gameWorldCreator.CanCreateNextGameWorld();

    public bool HasPreviousLevel => _gameWorldCreator.CanCreatePreviousGameWorld();

    public void Start()
    {
        _tickEngine.Continue();
        OpenMainMenu();
    }

    public void Update(float deltaTime)
    {
        _gameStateMachine.Update(deltaTime);
        _tickEngine.Tick(deltaTime);
    }

    #region Windows handlers
    public void ShowBackgroundGame()
    {
        _gameStateMachine.PushState(_backgroundGameState);
    }

    public void OpenMainMenu()
    {
        FinishPlayingState();

        _gameStateMachine.ClearStates();
        _gameStateMachine.PushState(_mainMenuState);
    }

    public void ActivateLevelSelection()
    {
        _gameStateMachine.PushState(_levelSelectionState);
    }

    public void BuildLevel(int indexOfLevel)
    {
        FinishPlayingState();
        PreparePlayingState(_gameWorldCreator.Create(indexOfLevel));
    }

    public void OpenOptions()
    {
        _gameStateMachine.PushState(_optionsMenuState);
    }

    public void OpenShop()
    {
        _gameStateMachine.PushState(_shopState);
    }

    public void Play()
    {
        _gameStateMachine.PushState(_playingState);
        _playingState.EnableGameWorld();
    }

    public void PlayNextLevel()
    {
        FinishPlayingState();
        PreparePlayingState(_gameWorldCreator.CreateNextGameWorld());
    }

    public void PlayPreviousLevel()
    {
        FinishPlayingState();
        PreparePlayingState(_gameWorldCreator.CreatePreviousGameWorld());
    }

    public void Return()
    {
        _gameStateMachine.PopState();
    }

    public void Pause()
    {
        _gameStateMachine.PushState(_pausedState);
    }

    public void Reset()
    {
        FinishPlayingState();
        PreparePlayingState(_gameWorldCreator.Recreate());
    }

    public void ActivateSwapAbility()
    {
        _swapAbilityState.Prepare(_playingState.GameWorld.BlockField);
        _gameStateMachine.PushState(_swapAbilityState);
    }
    #endregion

    private void PreparePlayingState(GameWorld gameWorld)
    {
        GameWorldCreated?.Invoke(gameWorld);

        _playingState.Prepare(gameWorld);

        _playingState.LevelPassed += OnLevelPassed;
        _playingState.LevelFailed += OnLevelFailed;

        Play();
    }

    private void FinishPlayingState()
    {
        _playingState.DisableGameWorld();

        _playingState.LevelPassed -= OnLevelPassed;
        _playingState.LevelFailed -= OnLevelFailed;

        _playingState.Clear();

        GameWorldDestroyed?.Invoke();
    }

    private void OnLevelPassed()
    {
        LevelPassed?.Invoke();
        _endLevelState.SetGameWorld(_playingState.GameWorld);
        _gameStateMachine.PushState(_endLevelState);
    }

    private void OnLevelFailed()
    {
        LevelFailed?.Invoke();
        _endLevelState.SetGameWorld(_playingState.GameWorld);
        _gameStateMachine.PushState(_endLevelState);
    }
}