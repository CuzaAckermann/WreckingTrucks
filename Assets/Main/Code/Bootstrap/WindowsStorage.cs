using System;
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

    private EventBus _eventBus;

    private bool _isInited;

    public void Init(EventBus eventBus,
                     StateStorage stateStorage,
                     AnimationSettings animationSettings,
                     TickEngine animationTickEngine,
                     int amountLevels)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        
        BindWindowToState(stateStorage, animationSettings, animationTickEngine, amountLevels);

        SubscribeToEventBus();

        _isInited = true;
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

    private void OnEnable()
    {
        if (_isInited == false)
        {
            return;
        }

        SubscribeToEventBus();
    }

    private void OnDisable()
    {
        if (_isInited == false)
        {
            return;
        }

        UnsubscribeFromEventBus();
    }

    private void BindWindowToState(StateStorage stateStorage,
                                   AnimationSettings animationSettings,
                                   TickEngine animationTickEngine,
                                   int amountLevels)
    {
        animationTickEngine.AddTickableCreator(_backgroundGameWindow);
        animationTickEngine.AddTickableCreator(_mainMenu);
        animationTickEngine.AddTickableCreator(_gameSelectionWindow);
        animationTickEngine.AddTickableCreator(_levelButtonsStorage);
        animationTickEngine.AddTickableCreator(_optionsMenu);
        animationTickEngine.AddTickableCreator(_shopWindow);
        animationTickEngine.AddTickableCreator(_playingWindow);
        animationTickEngine.AddTickableCreator(_swapAbilityWindow);
        animationTickEngine.AddTickableCreator(_pauseMenu);
        animationTickEngine.AddTickableCreator(_endLevelWindow);

        _backgroundGameWindow.Init(stateStorage.BackgroundGameState,
                                   animationSettings.AnimationSpeedForWindows);
        _mainMenu.Init(stateStorage.MainMenuState,
                       animationSettings.AnimationSpeedForWindows);
        _gameSelectionWindow.Init(stateStorage.GameSelectionState,
                                  animationSettings.AnimationSpeedForWindows);
        _levelButtonsStorage.Init(stateStorage.LevelSelectionState,
                                  animationSettings.AnimationSpeedForWindows,
                                  amountLevels);
        _optionsMenu.Init(stateStorage.OptionsMenuState,
                          animationSettings.AnimationSpeedForWindows);
        _shopWindow.Init(stateStorage.ShopState,
                         animationSettings.AnimationSpeedForWindows);
        _playingWindow.Init(stateStorage.PlayingState,
                            animationSettings.AnimationSpeedForWindows);
        _swapAbilityWindow.Init(stateStorage.SwapAbilityState,
                                animationSettings.AnimationSpeedForWindows);
        _pauseMenu.Init(stateStorage.PausedState,
                        animationSettings.AnimationSpeedForWindows);
        _endLevelWindow.Init(stateStorage.EndLevelState,
                             animationSettings.AnimationSpeedForWindows);
    }

    private void SubscribeToEventBus()
    {
        _eventBus.Subscribe<ClearedSignal<GameSignalEmitter>>(Clear);

        _eventBus.Subscribe<EnabledSignal<GameSignalEmitter>>(StartWork, Priority.High);
    }

    private void UnsubscribeFromEventBus()
    {
        _eventBus.Unsubscribe<ClearedSignal<GameSignalEmitter>>(Clear);

        _eventBus.Unsubscribe<EnabledSignal<GameSignalEmitter>>(StartWork);
    }

    private void Clear(ClearedSignal<GameSignalEmitter> _)
    {
        OnDisable();
    }

    private void StartWork(EnabledSignal<GameSignalEmitter> _)
    {
        HideAllWindows();
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