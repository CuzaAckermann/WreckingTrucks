using System;
using UnityEngine;

public class WindowsStorage : MonoBehaviour, ILevelSelectionWindowsStorage, IWindowsStorage
{
    [Header("Windows")]
    [SerializeField] private BackgroundGameWindow _backgroundGameWindow;
    [SerializeField] private MainMenuWindow _mainMenu;
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
                     BackgroundGameState backgroundGameState,
                     MainMenuInputState mainMenuState,
                     LevelSelectionState levelSelectionState, int amountLevels,
                     OptionsMenuState optionsMenuState,
                     ShopState shopState,
                     PlayingInputState playingState,
                     SwapAbilityState swapAbilityState,
                     PausedState pausedState,
                     EndLevelState endLevelState)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        
        BindWindowToState(backgroundGameState,
                          mainMenuState,
                          levelSelectionState, amountLevels,
                          optionsMenuState,
                          shopState,
                          playingState,
                          swapAbilityState,
                          pausedState,
                          endLevelState);

        SubscribeToEventBus();

        _isInited = true;
    }

    public BackgroundGameWindow BackgroundGameWindow => _backgroundGameWindow;

    public MainMenuWindow MainMenuWindow => _mainMenu;

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

    private void BindWindowToState(BackgroundGameState backgroundGameState,
                                   MainMenuInputState mainMenuState,
                                   LevelSelectionState levelSelectionState, int amountLevels,
                                   OptionsMenuState optionsMenuState,
                                   ShopState shopState,
                                   PlayingInputState playingState,
                                   SwapAbilityState swapAbilityState,
                                   PausedState pausedState,
                                   EndLevelState endLevelState)
    {
        _backgroundGameWindow.Init(backgroundGameState);
        _mainMenu.Init(mainMenuState);
        _levelButtonsStorage.Init(levelSelectionState, amountLevels);
        _optionsMenu.Init(optionsMenuState);
        _shopWindow.Init(shopState);
        _playingWindow.Init(playingState);
        _swapAbilityWindow.Init(swapAbilityState);
        _pauseMenu.Init(pausedState);
        _endLevelWindow.Init(endLevelState);
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
        _levelButtonsStorage.Hide();
        _optionsMenu.Hide();
        _shopWindow.Hide();
        _playingWindow.Hide();
        _pauseMenu.Hide();
        _endLevelWindow.Hide();
        _swapAbilityWindow.Hide();
    }
}