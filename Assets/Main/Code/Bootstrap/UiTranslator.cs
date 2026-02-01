using System;
using UnityEngine;

public class UiTranslator : MonoBehaviour, ILevelSelectionInformer, IWindowSwitchingInformer
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

        SubscribeToWindows();

        _isInited = true;
    }

    public event Action<int> IndexSelected;
    public event Action NextSelected;
    public event Action PreviousSelected;
    public event Action RecreateSelected;
    public event Action NonstopSelected;

    public event Action HideMainMenuActivated;
    public event Action MainMenuActivated;
    public event Action OptionsMenuActivated;
    public event Action ShopMenuActivated;
    public event Action PlayingMenuActivated;
    public event Action SwapAbilityMenuActivated;
    public event Action PauseMenuActivated;
    public event Action ReturnActivated;

    private void OnEnable()
    {
        if (_isInited == false)
        {
            return;
        }

        SubscribeToEventBus();

        SubscribeToWindows();
    }

    private void OnDisable()
    {
        if (_isInited == false)
        {
            return;
        }

        UnsubscribeFromEventBus();

        UnsubscribeFromWindows();
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

    #region Subscriptions / Unsubscriptions for Windows
    private void SubscribeToWindows()
    {
        _backgroundGameWindow.ShowMainMenuButtonPressed += OnMainMenuButtonPressed;

        _mainMenu.HideMenuButtonPressed += OnHideMainMenuButtonPressed;
        _mainMenu.PlayButtonPressed += OnPlayButtonPressed;
        _mainMenu.OptionsButtonPressed += OnOptionsButtonPressed;
        _mainMenu.ShopButtonPressed += OnShopButtonPressed;

        _levelButtonsStorage.ReturnButtonPressed += OnReturnButtonPressed;
        _levelButtonsStorage.LevelSelected += OnLevelSelected;
        _levelButtonsStorage.NonstopGameButtonPressed += OnNonstopGameButtonPressed;

        _optionsMenu.ReturnButtonPressed += OnMainMenuButtonPressed;

        _shopWindow.ReturnButtonPressed += OnReturnButtonPressed;

        _playingWindow.PauseButtonPressed += OnPauseButtonPressed;
        _playingWindow.SwapAbilityButtonPressed += OnSwapAbilityButtonPressed;

        _swapAbilityWindow.ReturnButtonPressed += OnReturnButtonPressed;

        _pauseMenu.MainMenuButtonPressed += OnMainMenuButtonPressed;
        _pauseMenu.ReturnButtonPressed += OnReturnButtonPressed;
        _pauseMenu.ResetLevelButtonPressed += OnResetButtonPressed;
        _pauseMenu.LevelSelectionButtonPressed += OnPlayButtonPressed;

        _endLevelWindow.MainMenuButtonPressed += OnMainMenuButtonPressed;
        _endLevelWindow.ResetLevelButtonPressed += OnResetButtonPressed;

        _endLevelWindow.LevelSelectionButtonPressed += OnPlayButtonPressed;
        _endLevelWindow.PreviousLevelButtonPressed += OnPreviousLevelPressed;
        _endLevelWindow.NextLevelButtonPressed += OnNextLevelPressed;
    }

    private void UnsubscribeFromWindows()
    {
        _backgroundGameWindow.ShowMainMenuButtonPressed -= OnMainMenuButtonPressed;

        _mainMenu.HideMenuButtonPressed -= OnHideMainMenuButtonPressed;
        _mainMenu.PlayButtonPressed -= OnPlayButtonPressed;
        _mainMenu.OptionsButtonPressed -= OnOptionsButtonPressed;
        _mainMenu.ShopButtonPressed -= OnShopButtonPressed;

        _levelButtonsStorage.ReturnButtonPressed -= OnReturnButtonPressed;
        _levelButtonsStorage.LevelSelected -= OnLevelSelected;
        _levelButtonsStorage.NonstopGameButtonPressed -= OnNonstopGameButtonPressed;

        _optionsMenu.ReturnButtonPressed -= OnMainMenuButtonPressed;

        _shopWindow.ReturnButtonPressed -= OnReturnButtonPressed;

        _playingWindow.PauseButtonPressed -= OnPauseButtonPressed;
        _playingWindow.SwapAbilityButtonPressed -= OnSwapAbilityButtonPressed;

        _swapAbilityWindow.ReturnButtonPressed -= OnReturnButtonPressed;

        _pauseMenu.MainMenuButtonPressed -= OnMainMenuButtonPressed;
        _pauseMenu.ReturnButtonPressed -= OnReturnButtonPressed;
        _pauseMenu.ResetLevelButtonPressed -= OnResetButtonPressed;
        _pauseMenu.LevelSelectionButtonPressed -= OnPlayButtonPressed;

        _endLevelWindow.MainMenuButtonPressed -= OnMainMenuButtonPressed;
        _endLevelWindow.ResetLevelButtonPressed -= OnResetButtonPressed;

        _endLevelWindow.LevelSelectionButtonPressed -= OnPlayButtonPressed;
        _endLevelWindow.PreviousLevelButtonPressed -= OnPreviousLevelPressed;
        _endLevelWindow.NextLevelButtonPressed -= OnNextLevelPressed;
    }
    #endregion

    #region Level Selection Informer events
    private void OnLevelSelected(int indexOfLevel)
    {
        IndexSelected?.Invoke(indexOfLevel);
    }

    private void OnNextLevelPressed()
    {
        NextSelected?.Invoke();
    }

    private void OnPreviousLevelPressed()
    {
        PreviousSelected?.Invoke();
    }

    private void OnResetButtonPressed()
    {
        RecreateSelected?.Invoke();
    }

    private void OnNonstopGameButtonPressed()
    {
        NonstopSelected?.Invoke();
    }
    #endregion

    #region Window Switching Informer events
    private void OnHideMainMenuButtonPressed()
    {
        HideMainMenuActivated?.Invoke();
    }

    private void OnMainMenuButtonPressed()
    {
        MainMenuActivated?.Invoke();
    }

    private void OnOptionsButtonPressed()
    {
        OptionsMenuActivated?.Invoke();
    }

    private void OnShopButtonPressed()
    {
        ShopMenuActivated?.Invoke();
    }

    private void OnPlayButtonPressed()
    {
        PlayingMenuActivated?.Invoke();
    }

    private void OnSwapAbilityButtonPressed()
    {
        SwapAbilityMenuActivated?.Invoke();
    }

    private void OnPauseButtonPressed()
    {
        PauseMenuActivated?.Invoke();
    }

    private void OnReturnButtonPressed()
    {
        ReturnActivated?.Invoke();
    }
    #endregion
}