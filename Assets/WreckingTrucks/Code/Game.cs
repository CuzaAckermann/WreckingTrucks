using UnityEngine;

public class Game : MonoBehaviour
{
    [Header("Windows")]
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private OptionsMenu _optionsMenu;
    [SerializeField] private PlayingWindow _playingWindow;
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private EndLevelWindow _endLevelWindow;

    [Header("Space Creators")]
    [SerializeField] private BlocksSpaceCreator _blocksSpaceCreator;
    [SerializeField] private TrucksSpaceCreator _trucksSpaceCreator;
    //[SerializeField] private RoadSpaceCreator _roadSpaceCreator;

    [Header("Input Creator")]
    [SerializeField] private KeyboardInputHandlerCreator _inputHandlerCreator;

    [Header("Generation")]
    [SerializeField] private LevelGenerator _levelGenerator;

    [Header("Interactable")]
    [SerializeField] private TruckPresenterDetector _truckPresenterDetector;

    [Header("Time")]
    [SerializeField, Range(0.001f, 0.1f)] private float _slowTimeScale = 0.1f;
    [SerializeField, Range(0.1f, 1)] private float _mediumTimeScale = 1;
    [SerializeField, Range(1, 100)] private float _hardTimeScale = 10;

    private GameWorldCreator _gameWorldCreator;

    private GameStateMachine _gameStateMachine;
    private MainMenuState _mainMenuState;
    private OptionsMenuState _optionsMenuState;
    private PlayingState _playingState;
    private PausedState _pausedState;
    private EndLevelState _endLevelState;

    private void Awake()
    {
        _blocksSpaceCreator.Initialize();
        _trucksSpaceCreator.Initialize();
        _levelGenerator.Initialize();

        _gameWorldCreator = new GameWorldCreator(_blocksSpaceCreator,
                                                 _trucksSpaceCreator);

        InitializeStates();
        InitializeWindows();
    }

    private void OnEnable()
    {
        SubscribeToWindows();
        //SubscribeToStates();
    }

    private void Start()
    {
        OnMainMenuButtonPressed();
    }

    private void Update()
    {
        _gameStateMachine.Update(Time.deltaTime * _slowTimeScale
                                                * _mediumTimeScale
                                                * _hardTimeScale);
    }

    private void OnDisable()
    {
        UnsubscribeFromWindows();
        //UnsubscribeFromStates();
    }

    private void InitializeStates()
    {
        _mainMenuState = new MainMenuState();
        _optionsMenuState = new OptionsMenuState();
        _playingState = new PlayingState(_inputHandlerCreator.CreatePlayingInputHandler(),
                                         _truckPresenterDetector);
        _pausedState = new PausedState(_inputHandlerCreator.CreatePauseInputHandler());
        _endLevelState = new EndLevelState();

        _gameStateMachine = new GameStateMachine();
    }

    private void InitializeWindows()
    {
        _mainMenu.Initialize(_mainMenuState);
        _optionsMenu.Initialize(_optionsMenuState);
        _playingWindow.Initialize(_playingState);
        _pauseMenu.Initialize(_pausedState);
        _endLevelWindow.Initialize(_endLevelState);
    }

    #region Windows callbacks
    private void OnMainMenuButtonPressed()
    {
        _gameStateMachine.ClearStates();
        _playingState.Clear();
        _gameStateMachine.PushState(_mainMenuState);
    }

    private void OnOptionsButtonPressed()
    {
        _gameStateMachine.PushState(_optionsMenuState);
    }

    private void OnPlayButtonPressed()
    {
        _playingState.PauseRequested += OnPauseButtonPressed;
        _playingState.Prepare(_gameWorldCreator.CreateGameWorld(_levelGenerator.GetLevelSettings()));
        _gameStateMachine.PushState(_playingState);
    }

    private void OnReturnButtonPressed()
    {
        _gameStateMachine.PopState();
    }

    private void OnPauseButtonPressed()
    {
        _playingState.PauseRequested -= OnPauseButtonPressed;
        _pausedState.PauseFinished += OnPauseFinished;
        _gameStateMachine.PushState(_pausedState);
    }

    private void OnResetButtonPressed()
    {
        _playingState.Clear();
        _playingState.Prepare(_gameWorldCreator.CreateGameWorld(_levelGenerator.GetLevelSettings()));
        _gameStateMachine.PushState(_playingState);
    }
    #endregion

    #region UI Subscribes / Unsubscribes
    private void SubscribeToWindows()
    {
        _mainMenu.PlayButtonPressed += OnPlayButtonPressed;
        _mainMenu.OptionsButtonPressed += OnOptionsButtonPressed;

        _optionsMenu.ReturnButtonPressed += OnMainMenuButtonPressed;

        _playingWindow.PauseButtonPressed += OnPauseButtonPressed;

        _pauseMenu.MainMenuButtonPressed += OnMainMenuButtonPressed;
        _pauseMenu.ReturnButtonPressed += OnReturnButtonPressed;
        _pauseMenu.ResetLevelButtonPressed += OnResetButtonPressed;

        _endLevelWindow.MainMenuButtonPressed += OnMainMenuButtonPressed;
        _endLevelWindow.ResetLevelButtonPressed += OnResetButtonPressed;
    }

    private void UnsubscribeFromWindows()
    {
        _mainMenu.PlayButtonPressed -= OnPlayButtonPressed;
        _mainMenu.OptionsButtonPressed -= OnOptionsButtonPressed;

        _optionsMenu.ReturnButtonPressed -= OnMainMenuButtonPressed;

        _playingWindow.PauseButtonPressed -= OnPauseButtonPressed;

        _pauseMenu.MainMenuButtonPressed -= OnMainMenuButtonPressed;
        _pauseMenu.ReturnButtonPressed -= OnReturnButtonPressed;
        _pauseMenu.ResetLevelButtonPressed -= OnResetButtonPressed;

        _endLevelWindow.MainMenuButtonPressed -= OnMainMenuButtonPressed;
        _endLevelWindow.ResetLevelButtonPressed -= OnResetButtonPressed;
    }
    #endregion

    #region States callbacks
    private void OnLevelPassed()
    {
        _gameStateMachine.PushState(_endLevelState);
    }

    private void OnPauseFinished()
    {
        _pausedState.PauseFinished -= OnPauseFinished;
        _playingState.PauseRequested += OnPauseButtonPressed;
        _gameStateMachine.PushState(_playingState);
    }
    #endregion

    #region States Subscribes / Unsubscribes
    //private void SubscribeToStates()
    //{
    //    _playingState.LevelPassed += OnLevelPassed;
    //}

    //private void UnsubscribeFromStates()
    //{
    //    _playingState.LevelPassed -= OnLevelPassed;
    //}
    #endregion
}