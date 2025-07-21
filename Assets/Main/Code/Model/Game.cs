using System;

public class Game
{
    private readonly TickEngine _tickEngine;
    
    private GameStateMachine _gameStateMachine;

    private BackgroundGameState _backgroundGameState;
    private MainMenuState _mainMenuState;
    private LevelSelectionState _levelSelectionState;
    private OptionsMenuState _optionsMenuState;
    private PlayingState _playingState;
    private SwapAbilityState _swapAbilityState;
    private PausedState _pausedState;
    private EndLevelState _endLevelState;

    public Game(TickEngine tickEngine,
                BackgroundGameState backgroundGameState,
                MainMenuState mainMenuState,
                LevelSelectionState levelSelectionState,
                OptionsMenuState optionsMenuState,
                PlayingState playingState,
                SwapAbilityState swapAbilityState,
                PausedState pausedState,
                EndLevelState endLevelState)
    {
        _tickEngine = tickEngine ?? throw new ArgumentNullException(nameof(tickEngine));

        _backgroundGameState = backgroundGameState ?? throw new ArgumentNullException(nameof(backgroundGameState));
        _mainMenuState = mainMenuState ?? throw new ArgumentNullException(nameof(mainMenuState));
        _levelSelectionState = levelSelectionState ?? throw new ArgumentNullException(nameof(levelSelectionState));
        _optionsMenuState = optionsMenuState ?? throw new ArgumentNullException(nameof(optionsMenuState));
        _playingState = playingState ?? throw new ArgumentNullException(nameof(playingState));
        _swapAbilityState = swapAbilityState ?? throw new ArgumentNullException(nameof(swapAbilityState));
        _pausedState = pausedState ?? throw new ArgumentNullException(nameof(pausedState));
        _endLevelState = endLevelState ?? throw new ArgumentNullException(nameof(endLevelState));

        _gameStateMachine = new GameStateMachine();
    }

    public event Action LevelPassed;
    public event Action LevelFailed;

    public void PrepareLevel(GameWorld gameWorld,
                             FillingCard blockFillingCard,
                             FillingCard truckFillingCard,
                             FillingCard cartrigeBoxFillingCard)
    {
        _playingState.Prepare(gameWorld,
                              blockFillingCard,
                              truckFillingCard,
                              cartrigeBoxFillingCard);
    }

    public void Update(float deltaTime)
    {
        _gameStateMachine.Update(deltaTime);
        _tickEngine.Tick(deltaTime);
    }

    #region Windows handlers
    public void ActivateBackgroundGame()
    {
        _gameStateMachine.PushState(_backgroundGameState);
    }

    public void ActivateMainMenu()
    {
        _playingState.Clear();

        _gameStateMachine.ClearStates();

        _gameStateMachine.PushState(_mainMenuState);
    }

    public void ActivateOptions()
    {
        _gameStateMachine.PushState(_optionsMenuState);
    }

    public void PrepareLevel()
    {
        _gameStateMachine.PushState(_levelSelectionState);
    }

    public void Play()
    {
        _tickEngine.Continue();
        _gameStateMachine.PushState(_playingState);
    }

    public void ActivateSwapAbility()
    {
        _gameStateMachine.PushState(_swapAbilityState);
    }

    public void Return()
    {
        _gameStateMachine.PopState();
    }

    public void Pause()
    {
        _gameStateMachine.PushState(_pausedState);
        _tickEngine.Pause();
    }

    public void Reset()
    {
        _gameStateMachine.PushState(_playingState);
    }
    #endregion

    private void OnLevelPassed()
    {
        LevelPassed?.Invoke();
    }

    private void OnLevelFailed()
    {
        LevelFailed?.Invoke();
    }
}