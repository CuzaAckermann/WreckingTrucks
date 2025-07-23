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
        _playingState = playingState ?? throw new ArgumentNullException(nameof(playingState));
        _swapAbilityState = swapAbilityState ?? throw new ArgumentNullException(nameof(swapAbilityState));
        _pausedState = pausedState ?? throw new ArgumentNullException(nameof(pausedState));
        _endLevelState = endLevelState ?? throw new ArgumentNullException(nameof(endLevelState));
    }

    // —обыти€ нужны дл€ отображени€ нужного окна

    public event Action LevelPassed;
    public event Action LevelFailed;

    public void Update(float deltaTime)
    {
        _gameStateMachine.Update(deltaTime);
        _tickEngine.Tick(deltaTime);
    }

    #region Windows handlers
    public void ShowFullScreenBackgroundGame()
    {
        _gameStateMachine.PushState(_backgroundGameState);
    }

    public void ActivateMainMenu()
    {
        _playingState.LevelPassed -= OnLevelPassed;
        _playingState.LevelFailed -= OnLevelFailed;

        _playingState.Clear();

        _gameStateMachine.ClearStates();

        _gameStateMachine.PushState(_mainMenuState);
    }

    public void ActivateLevelSelection()
    {
        _gameStateMachine.PushState(_levelSelectionState);
    }

    public void BuildLevel(GameWorldSettings gameWorldSettings)
    {
        _playingState.Prepare(_gameWorldCreator.Create(gameWorldSettings));

        _playingState.LevelPassed += OnLevelPassed;
        _playingState.LevelFailed += OnLevelFailed;

        Play();
    }

    public void ActivateOptions()
    {
        _gameStateMachine.PushState(_optionsMenuState);
    }

    public void Play()
    {
        _tickEngine.Continue();
        _gameStateMachine.PushState(_playingState);
    }

    public void Return()
    {
        _gameStateMachine.PopState();

        // удалить
        _tickEngine.Continue();
    }

    public void Pause()
    {
        _tickEngine.Pause();
        _gameStateMachine.PushState(_pausedState);
    }

    public void Reset()
    {
        _playingState.LevelPassed -= OnLevelPassed;
        _playingState.LevelFailed -= OnLevelFailed;

        _playingState.Clear();

        _playingState.LevelPassed += OnLevelPassed;
        _playingState.LevelFailed += OnLevelFailed;

        _playingState.Prepare(_gameWorldCreator.Recreate());
        
        Return();

        _tickEngine.Continue();
    }

    public void ActivateSwapAbility()
    {
        _swapAbilityState.Prepare(_playingState.GameWorld.BlockField);
        _gameStateMachine.PushState(_swapAbilityState);
    }
    #endregion
    private void OnLevelPassed()
    {
        _gameStateMachine.PushState(_endLevelState);
    }

    private void OnLevelFailed()
    {
        _gameStateMachine.PushState(_endLevelState);
    }
}