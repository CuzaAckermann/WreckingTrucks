using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    [Header("Windows")]
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private OptionsMenu _optionsMenu;
    [SerializeField] private PlayingWindow _playingWindow;
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private EndLevelWindow _endLevelWindow;

    [Header("Engines")]
    [SerializeField] private TickEngineUpdater _tickEngineUpdater;
    [SerializeField] private Productions _productions;

    [SerializeField] private TextMeshProUGUI _textMeshPro;

    private LevelStarter _levelStarter;

    private GameStateMachine _gameStateMachine;

    private MainMenuState _mainMenuState;
    private OptionsMenuState _optionsMenuState;
    private PlayingState _playingState;
    private PausedState _pausedState;
    private EndLevelState _endLevelState;

    private int _amount = 0;

    private void Awake()
    {
        _productions.Initialize();
        _tickEngineUpdater.Initialize();
        //_levelStarter.Initialize(_productions, _tickEngineUpdater);
        _tickEngineUpdater.Pause();

        _mainMenuState = new MainMenuState(_mainMenu);
        _optionsMenuState = new OptionsMenuState(_optionsMenu);
        _playingState = new PlayingState(_playingWindow, _levelStarter);
        _pausedState = new PausedState(_pauseMenu);
        _endLevelState = new EndLevelState(_endLevelWindow);

        _gameStateMachine = new GameStateMachine(_mainMenuState);
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void Start()
    {
        OnMainMenuButtonPressed();
    }

    private void Update()
    {
        _gameStateMachine.Update(Time.deltaTime);
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    #region Event Callbacks
    private void OnMainMenuButtonPressed()
    {
        _gameStateMachine.SwitchState(_mainMenuState);
    }

    private void OnOptionsButtonPressed()
    {
        _gameStateMachine.SwitchState(_optionsMenuState);
    }

    private void OnPlayButtonPressed()
    {
        _gameStateMachine.SwitchState(_playingState);
    }

    private void OnReturnButtonPressed()
    {
        _gameStateMachine.ReturnToPreviousState();
    }

    private void OnPauseButtonPressed()
    {
        _gameStateMachine.SwitchState(_pausedState);
    }

    private void OnResetButtonPressed()
    {
        _amount = 0;
        _playingState.ResetLevel();
        _gameStateMachine.SwitchState(_playingState);
    }

    private void OnLevelPassed()
    {
        _gameStateMachine.SwitchState(_endLevelState);
    }

    private void OnIntervalPassed()
    {
        _amount++;
        _textMeshPro.text = _amount.ToString();
    }
    #endregion

    #region UI Subscribes / Unsubscribes
    private void Subscribe()
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

        _playingState.IntervalPassed += OnIntervalPassed;
    }

    private void Unsubscribe()
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

        _playingState.IntervalPassed -= OnIntervalPassed;
    }
    #endregion
}