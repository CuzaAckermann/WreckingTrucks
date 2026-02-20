using UnityEngine;

public class WindowsStorage : MonoBehaviour, ILevelSelectionWindowsStorage, IWindowsStorage
{
    [Header("Windows")]
    [SerializeField] private BackgroundGameWindow _backgroundGameWindow;
    [SerializeField] private MainMenuWindow _mainMenu;
    [SerializeField] private GameSelectionWindow _gameSelectionWindow;
    [SerializeField] private LevelButtonsStorage _levelButtonsStorage;
    [SerializeField] private OptionsMenu _optionsMenu;
    [SerializeField] private ShopWindow _shopWindow;
    [SerializeField] private PlayingWindow _playingWindow;
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private EndLevelWindow _endLevelWindow;
    [SerializeField] private SwapAbilityWindow _swapAbilityWindow;

    public void Init(InputStateStorage stateStorage,
                     AnimationSettings animationSettings,
                     int amountLevels)
    {
        BindWindowToState(stateStorage, animationSettings, amountLevels);

        HideAllWindows();
    }

    public BackgroundGameWindow BackgroundGameWindow => _backgroundGameWindow;

    public MainMenuWindow MainMenuWindow => _mainMenu;

    public GameSelectionWindow GameSelectionWindow => _gameSelectionWindow;

    public LevelButtonsStorage LevelButtonsStorage => _levelButtonsStorage;

    public OptionsMenu OptionsMenu => _optionsMenu;

    public ShopWindow ShopWindow => _shopWindow;

    public PlayingWindow PlayingWindow => _playingWindow;

    public PauseMenu PauseMenu => _pauseMenu;

    public EndLevelWindow EndLevelWindow => _endLevelWindow;

    public SwapAbilityWindow SwapAbilityWindow => _swapAbilityWindow;

    private void BindWindowToState(InputStateStorage stateStorage,
                                   AnimationSettings animationSettings,
                                   int amountLevels)
    {
        //animationTickEngine.AddTickableCreator(_backgroundGameWindow);
        //animationTickEngine.AddTickableCreator(_mainMenu);
        //animationTickEngine.AddTickableCreator(_gameSelectionWindow);
        //animationTickEngine.AddTickableCreator(_levelButtonsStorage);
        //animationTickEngine.AddTickableCreator(_optionsMenu);
        //animationTickEngine.AddTickableCreator(_shopWindow);
        //animationTickEngine.AddTickableCreator(_playingWindow);
        //animationTickEngine.AddTickableCreator(_swapAbilityWindow);
        //animationTickEngine.AddTickableCreator(_pauseMenu);
        //animationTickEngine.AddTickableCreator(_endLevelWindow);

        _backgroundGameWindow.Init(stateStorage.ComputerGameplayInputState,
                                   animationSettings.AnimationSpeedForWindows);
        _mainMenu.Init(stateStorage.MainMenuInputState,
                       animationSettings.AnimationSpeedForWindows);
        _gameSelectionWindow.Init(stateStorage.GameSelectionInputState,
                                  animationSettings.AnimationSpeedForWindows);
        _levelButtonsStorage.Init(stateStorage.LevelSelectionInputState,
                                  animationSettings.AnimationSpeedForWindows,
                                  amountLevels);
        _optionsMenu.Init(stateStorage.OptionsMenuInputState,
                          animationSettings.AnimationSpeedForWindows);
        _shopWindow.Init(stateStorage.ShopInputState,
                         animationSettings.AnimationSpeedForWindows);
        _playingWindow.Init(stateStorage.PlayingInputState,
                            animationSettings.AnimationSpeedForWindows);
        _swapAbilityWindow.Init(stateStorage.SwapAbilityInputState,
                                animationSettings.AnimationSpeedForWindows);
        _pauseMenu.Init(stateStorage.PausedInputState,
                        animationSettings.AnimationSpeedForWindows);
        _endLevelWindow.Init(stateStorage.EndLevelInputState,
                             animationSettings.AnimationSpeedForWindows);
    }

    private void HideAllWindows()
    {
        _backgroundGameWindow.Hide();
        _mainMenu.Hide();
        _gameSelectionWindow.Hide();
        _levelButtonsStorage.Hide();
        _optionsMenu.Hide();
        _shopWindow.Hide();
        _playingWindow.Hide();
        _pauseMenu.Hide();
        _endLevelWindow.Hide();
        _swapAbilityWindow.Hide();
    }
}