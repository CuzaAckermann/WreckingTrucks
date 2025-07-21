using System;

public class GameTEMP
{
    //private GameWorldCreator _gameWorldCreator;

    //private GameStateMachine _gameStateMachine;
    //private BackgroundGameState _backgroundGameState;
    //private MainMenuState _mainMenuState;
    //private OptionsMenuState _optionsMenuState;
    //private PlayingState _playingState;
    //private SwapAbilityStateTEMP _swapAbilityState;
    //private PausedState _pausedState;
    //private EndLevelState _endLevelState;

    //private BackgroundGameTEMP _backgruondGame;
    //private bool _isBackgruondGameEnabled = false;

    //public GameTEMP(BackgroundGameState backgroundGameState,
    //            MainMenuState mainMenuState,
    //            OptionsMenuState optionsMenuState,
    //            PlayingState playingState,
    //            SwapAbilityStateTEMP swapAbilityState,
    //            PausedState pausedState,
    //            EndLevelState endLevelState,
    //            GameWorldCreator gameWorldCreator,
    //            BackgroundGameTEMP backgroundGame)
    //{
    //    _backgroundGameState = backgroundGameState ?? throw new ArgumentNullException(nameof(backgroundGameState));
    //    _mainMenuState = mainMenuState ?? throw new ArgumentNullException(nameof(mainMenuState));
    //    _optionsMenuState = optionsMenuState ?? throw new ArgumentNullException(nameof(optionsMenuState));
    //    _playingState = playingState ?? throw new ArgumentNullException(nameof(playingState));
    //    _swapAbilityState = swapAbilityState ?? throw new ArgumentNullException(nameof(swapAbilityState));
    //    _pausedState = pausedState ?? throw new ArgumentNullException(nameof(pausedState));
    //    _endLevelState = endLevelState ?? throw new ArgumentNullException(nameof(endLevelState));

    //    _gameStateMachine = new GameStateMachine();

    //    _gameWorldCreator = gameWorldCreator ?? throw new ArgumentNullException(nameof(gameWorldCreator));
    //    _backgruondGame = backgroundGame ?? throw new ArgumentNullException(nameof(backgroundGame));
    //}

    //public void Start()
    //{
    //    RunMainMenu();
    //}

    //public void Update(float deltaTime)
    //{
    //    _gameStateMachine.Update(deltaTime);
    //    _backgruondGame.Update(deltaTime);
    //}

    //private void RunBackgroundGame()
    //{
    //    if (_isBackgruondGameEnabled == false)
    //    {
    //        _backgruondGame.Prepare(_gameWorldCreator.CreateGameWorld(spaceSettings));
    //        _backgruondGame.Start();
    //        _isBackgruondGameEnabled = true;
    //    }
    //}

    //public void CompleteBackgroundGame()
    //{
    //    if (_isBackgruondGameEnabled)
    //    {
    //        _backgruondGame.Stop();
    //        _backgruondGame.Clear();
    //        _isBackgruondGameEnabled = false;
    //    }
    //}

    //public void RunPlayingState()
    //{
    //    SubscribeToStates();
    //    _playingState.PauseRequested += Pause;
    //    _playingState.Prepare(_gameWorldCreator.CreateGameWorld(spaceSettings));
    //    _gameStateMachine.PushState(_playingState);
    //}

    //private void CompletePlayingState()
    //{
    //    UnsubscribeFromStates();
    //    _playingState.Clear();
    //}

    //#region Windows callbacks
    //public void ActivateBackgroundGame()
    //{
    //    _gameStateMachine.PushState(_backgroundGameState);
    //}

    //public void RunMainMenu()
    //{
    //    _gameStateMachine.ClearStates();
    //    CompletePlayingState();

    //    RunBackgroundGame();

    //    _gameStateMachine.PushState(_mainMenuState);
    //}

    //public void ActivateOptions()
    //{
    //    _gameStateMachine.PushState(_optionsMenuState);
    //}

    //public void Play()
    //{
    //    CompleteBackgroundGame();
    //    RunPlayingState();
    //}

    //public void ActivateSwapAbility()
    //{
    //    _swapAbilityState.Prepare(_playingState.GameWorld.BlockField);
    //    _gameStateMachine.PushState(_swapAbilityState);
    //}

    //public void Return()
    //{
    //    _gameStateMachine.PopState();
    //}

    //public void Pause()
    //{
    //    _playingState.PauseRequested -= Pause;
    //    _pausedState.PauseFinished += OnPauseFinished;
    //    _gameStateMachine.PushState(_pausedState);
    //}

    //public void Reset()
    //{
    //    _playingState.Clear();
    //    _playingState.Prepare(_gameWorldCreator.CreateGameWorld(spaceSettings));
    //    _gameStateMachine.PushState(_playingState);
    //}
    //#endregion

    //#region States callbacks
    //private void OnLevelPassed()
    //{
    //    _gameStateMachine.PushState(_endLevelState);
    //}

    //private void OnPauseFinished()
    //{
    //    _pausedState.PauseFinished -= OnPauseFinished;
    //    _playingState.PauseRequested += Pause;
    //    _gameStateMachine.PushState(_playingState);
    //}
    //#endregion

    //#region States Subscribes / Unsubscribes
    //private void SubscribeToStates()
    //{
    //    _playingState.LevelPassed += OnLevelPassed;
    //    _swapAbilityState.AbilityFinished += Return;
    //}

    //private void UnsubscribeFromStates()
    //{
    //    _playingState.LevelPassed -= OnLevelPassed;
    //    _swapAbilityState.AbilityFinished -= Return;
    //}
    //#endregion
}