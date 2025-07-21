using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [Header("Windows")]
    [SerializeField] private BackgroundGameWindow _backgroundGameWindow;
    [SerializeField] private MainMenu _mainMenuWindow;
    [SerializeField] private LevelButtonsStorage _levelButtonsStorage;
    [SerializeField] private OptionsMenu _optionsMenuWindow;
    [SerializeField] private PlayingWindow _playingWindow;
    [SerializeField] private PauseMenu _pauseMenuWindow;
    [SerializeField] private EndLevelWindow _endLevelWindow;
    [SerializeField] private SwapAbilityWindow _swapAbilityWindow;

    [Header("Presenter Production Creator")]
    [SerializeField] private PresenterProductionCreator _presenterProductionCreator;

    [Header("Settings")]
    [Header("Factory Settings")]
    [SerializeField] private ModelFactoriesSettings _modelFactoriesSettings;

    [Header("Space Settings")]
    [SerializeField] private PlacementSettings _placementSettings;

    [Header("Path Settings")]
    [SerializeField] private PathSettings _pathSettings;

    [Header("Game World Settings")]
    [SerializeField] private GameWorldSettings _gameWorldSettings;

    [Header("Filler Creator Settings")]
    [SerializeField] private FillerCreatorSettings _fillerCreatorSettings;

    [Header("Storage Level Settings")]
    [SerializeField] private StorageLevelSettings _storageLevelSettings;

    [Header("Input Settings")]
    [SerializeField] private KeyboardInputSettings _keyboardInputSettings;

    [Header("Detectors")]
    [SerializeField] private TruckPresenterDetector _truckPresenterDetector;
    [SerializeField] private BlockPresenterDetector _blockPresenterDetector;

    [Header("Application Configurator")]
    [SerializeField] private ApplicationConfigurator _applicationConfigurator;
    [SerializeField] private DeltaTimeCalculator _deltaTimeCalculator;
    [SerializeField] private PositionCorrector _positionCorrector;

    // PREPARING

    private Game _game;
    private TickEngine _tickEngine;

    // MAIN CREATORS

    private ModelProductionCreator _modelProductionCreator;
    private GameWorldCreator _gameWorldCreator;

    // SPACE CREATORS

    private BlockSpaceCreator _blockSpaceCreator;
    private TruckSpaceCreator _truckSpaceCreator;
    private CartrigeBoxSpaceCreator _cartrigeBoxSpaceCreator;
    private RoadSpaceCreator _roadSpaceCreator;
    private ShootingSpaceCreator _shootingSpaceCreator;
    private SupplierSpaceCreator _supplierSpaceCreator;

    // ELEMENTS CREATORS

    private ChargerCreator _chargerCreator;
    private BulletSimulationCreator _bulletSimulationCreator;
    private SupplierCreator _supplierCreator;
    private RotatorCreator _rotatorCreator;
    private PathCreator _pathCreator;
    private RoadCreator _roadCreator;
    private ModelFinalizerCreator _modelFinalizerCreator;
    private ColumnCreator _columnCreator;
    private LayerCreator _layerCreator;
    private BinderCreator _binderCreator;
    private BlockFieldCreator _fieldCreator;
    private MoverCreator _moverCreator;
    private FillerCreator _fillerCreator;
    // private AIClickerCreator _AIClickerCreator;

    private TruckFieldCreator _truckFieldCreator;
    private TruckGeneratorCreator _truckGeneratorCreator;

    private CartrigeBoxFieldCreator _cartrigeBoxFieldCreator;

    // тут SwapAbilityCreator

    private StopwatchCreator _stopwatchCreator;

    // SETTINGS CREATORS

    private GameWorldSettingsCreator _gameWorldSettingsCreator;

    // FILLING CARD CREATORS

    private BlockFillingCardCreator _blockFillingCardCreator;
    private TruckFillingCardCreator _truckFillingCardCreator;
    private CartrigeBoxFillingCardCreator _cartrigeBoxFillingCardCreator;

    // INPUT CREATOR

    private KeyboardInputHandlerCreator _keyboardInputHandlerCreator;

    // CONVERTERS

    private BlockTypeConverter _blockTypeConverter;
    private TruckTypeConverter _truckTypeConverter;
    private CartrigeBoxTypeConverter _cartrigeBoxTypeConverter;

    // MAIN STATES

    private BackgroundGameState _backgroundGameState;
    private MainMenuState _mainMenuState;
    private LevelSelectionState _levelSelectionState;
    private OptionsMenuState _optionsMenuState;
    private PlayingState _playingState;
    private PausedState _pausedState;
    private EndLevelState _endLevelState;

    // ABILITY STATES

    private SwapAbilityState _swapAbilityState;

    private void Awake()
    {
        // корректировка

        _positionCorrector.CorrectTransformable(_placementSettings.TruckField, _gameWorldSettings.TruckSpaceSettings.FieldSettings);

        // корректировка

        _applicationConfigurator.ConfigureApplication();
        HideAllWindows();

        InitializeTime();
        InitializeConverters();
        InitializeSettingsCreators();
        InitializeElementCreators();
        InitializeSpaceCreators();
        InitailizeMainCreators();
        InitializeGameState();
        InitializeWindows();
        InitializeGame();
    }

    private void OnEnable()
    {
        SubscribeToWindows();
        SubscribeToLevelButtonStorage();
    }

    private void Start()
    {
        OnMainMenuButtonPressed();
    }

    private void Update()
    {
        _game.Update(_deltaTimeCalculator.GetDeltaTime());
    }

    private void OnDisable()
    {
        UnsubscribeFromWindows();
        UnsubscribeFromLevelButtonStorage();
    }

    private void HideAllWindows()
    {
        _backgroundGameWindow.Hide();
        _mainMenuWindow.Hide();
        _levelButtonsStorage.Hide();
        _optionsMenuWindow.Hide();
        _playingWindow.Hide();
        _pauseMenuWindow.Hide();
        _endLevelWindow.Hide();
        _swapAbilityWindow.Hide();
    }

    #region Initializations
    private void InitializeConverters()
    {
        _blockTypeConverter = new BlockTypeConverter();
        _truckTypeConverter = new TruckTypeConverter();
        _cartrigeBoxTypeConverter = new CartrigeBoxTypeConverter();
    }

    private void InitializeTime()
    {
        _deltaTimeCalculator = new DeltaTimeCalculator();
        _tickEngine = new TickEngine();
        _stopwatchCreator = new StopwatchCreator(_tickEngine);
    }

    private void InitializeSettingsCreators()
    {
        _gameWorldSettingsCreator = new GameWorldSettingsCreator(_gameWorldSettings);
        _modelProductionCreator = new ModelProductionCreator(_modelFactoriesSettings,
                                                             new TrunkCreator(),
                                                             new BlockTrackerCreator(_gameWorldSettings.BlockTrackerCreatorSettings.AcceptableAngle),
                                                             _stopwatchCreator);

        _blockFillingCardCreator = new BlockFillingCardCreator(_modelProductionCreator.CreateBlockProduction(),
                                                               _blockTypeConverter);
        _truckFillingCardCreator = new TruckFillingCardCreator(_modelProductionCreator.CreateTruckProduction(),
                                                               _truckTypeConverter);
        _cartrigeBoxFillingCardCreator = new CartrigeBoxFillingCardCreator(_modelProductionCreator.CreateCartrigeBoxProduction(),
                                                                           _cartrigeBoxTypeConverter);
        _presenterProductionCreator.Initialize();
    }

    private void InitializeElementCreators()
    {
        _chargerCreator = new ChargerCreator(_modelProductionCreator.CreateBulletFactory());
        _bulletSimulationCreator = new BulletSimulationCreator(_chargerCreator);
        _supplierCreator = new SupplierCreator();
        _rotatorCreator = new RotatorCreator(_tickEngine);
        _pathCreator = new PathCreator();
        _roadCreator = new RoadCreator(_pathCreator);
        _modelFinalizerCreator = new ModelFinalizerCreator();
        _columnCreator = new ColumnCreator();
        _layerCreator = new LayerCreator(_columnCreator);
        _binderCreator = new BinderCreator();
        _fieldCreator = new BlockFieldCreator(_layerCreator);
        _moverCreator = new MoverCreator(_tickEngine);
        _fillerCreator = new FillerCreator(_fillerCreatorSettings, _stopwatchCreator);

        _truckFieldCreator = new TruckFieldCreator(_layerCreator);
        _truckGeneratorCreator = new TruckGeneratorCreator(new CreatedTypesCreator<TruckTypeConverter>(_truckTypeConverter));

        _cartrigeBoxFieldCreator = new CartrigeBoxFieldCreator(_layerCreator);
    }

    private void InitializeSpaceCreators()
    {
        _blockSpaceCreator = new BlockSpaceCreator(_fieldCreator,
                                                   _moverCreator,
                                                   _fillerCreator,
                                                   _modelFinalizerCreator);
        _truckSpaceCreator = new TruckSpaceCreator(_truckFieldCreator,
                                                   _moverCreator,
                                                   _fillerCreator,
                                                   _modelFinalizerCreator,
                                                   _modelProductionCreator,
                                                   _truckGeneratorCreator);
        _cartrigeBoxSpaceCreator = new CartrigeBoxSpaceCreator(_cartrigeBoxFieldCreator,
                                                               _moverCreator,
                                                               _fillerCreator,
                                                               _modelFinalizerCreator);
        _roadSpaceCreator = new RoadSpaceCreator(_roadCreator,
                                                 _moverCreator,
                                                 _rotatorCreator,
                                                 _modelFinalizerCreator);
        _shootingSpaceCreator = new ShootingSpaceCreator(_bulletSimulationCreator,
                                                         _moverCreator,
                                                         _rotatorCreator);
        _supplierSpaceCreator = new SupplierSpaceCreator(_supplierCreator,
                                                         _moverCreator);
    }

    private void InitailizeMainCreators()
    {
        _gameWorldCreator = new GameWorldCreator(_blockSpaceCreator,
                                                 _truckSpaceCreator,
                                                 _cartrigeBoxSpaceCreator,
                                                 _roadSpaceCreator,
                                                 _shootingSpaceCreator,
                                                 _supplierSpaceCreator,
                                                 _binderCreator.Create(_presenterProductionCreator.CreatePresenterProduction()));

        _keyboardInputHandlerCreator = new KeyboardInputHandlerCreator(_keyboardInputSettings);
    }

    private void InitializeGameState()
    {
        _backgroundGameState = new BackgroundGameState();
        _mainMenuState = new MainMenuState();
        _levelSelectionState = new LevelSelectionState();
        _optionsMenuState = new OptionsMenuState();
        _playingState = new PlayingState(_truckPresenterDetector,
                                         _keyboardInputHandlerCreator.CreatePlayingInputHandler());
        _swapAbilityState = new SwapAbilityState(_blockPresenterDetector,
                                                 _keyboardInputHandlerCreator.CreateSwapAbilityInputHandler());
        _pausedState = new PausedState(_keyboardInputHandlerCreator.CreatePauseInputHandler());
        _endLevelState = new EndLevelState();
    }

    private void InitializeWindows()
    {
        _backgroundGameWindow.Initialize(_backgroundGameState);
        _mainMenuWindow.Initialize(_mainMenuState);
        _levelButtonsStorage.Initailize(_levelSelectionState, _storageLevelSettings.AmountLevels);
        _optionsMenuWindow.Initialize(_optionsMenuState);
        _playingWindow.Initialize(_playingState);
        _swapAbilityWindow.Initialize(_swapAbilityState);
        _pauseMenuWindow.Initialize(_pausedState);
        _endLevelWindow.Initialize(_endLevelState);
    }

    private void InitializeGame()
    {
        _game = new Game(_tickEngine,
                         _backgroundGameState,
                         _mainMenuState,
                         _levelSelectionState,
                         _optionsMenuState,
                         _playingState,
                         _swapAbilityState,
                         _pausedState,
                         _endLevelState);
    }
    #endregion

    #region UI Subscribes / Unsubscribes
    private void SubscribeToWindows()
    {
        _backgroundGameWindow.ShowMainMenuButtonPressed += OnMainMenuButtonPressed;

        _mainMenuWindow.PlayButtonPressed += OnPlayButtonPressed;
        _mainMenuWindow.OptionsButtonPressed += OnOptionsButtonPressed;
        _mainMenuWindow.HideMenuButtonPressed += OnHideMainMenuButtonPressed;

        _optionsMenuWindow.ReturnButtonPressed += OnMainMenuButtonPressed;

        _playingWindow.PauseButtonPressed += OnPauseButtonPressed;
        _playingWindow.SwapAbilityButtonPressed += OnSwapAbilityButtonPressed;

        _swapAbilityWindow.ReturnButtonPressed += OnReturnButtonPressed;

        _pauseMenuWindow.MainMenuButtonPressed += OnMainMenuButtonPressed;
        _pauseMenuWindow.ReturnButtonPressed += OnReturnButtonPressed;
        _pauseMenuWindow.ResetLevelButtonPressed += OnResetButtonPressed;

        _endLevelWindow.MainMenuButtonPressed += OnMainMenuButtonPressed;
        _endLevelWindow.ResetLevelButtonPressed += OnResetButtonPressed;
    }

    private void UnsubscribeFromWindows()
    {
        _backgroundGameWindow.ShowMainMenuButtonPressed -= OnMainMenuButtonPressed;

        _mainMenuWindow.PlayButtonPressed -= OnPlayButtonPressed;
        _mainMenuWindow.OptionsButtonPressed -= OnOptionsButtonPressed;
        _mainMenuWindow.HideMenuButtonPressed -= OnHideMainMenuButtonPressed;

        _optionsMenuWindow.ReturnButtonPressed -= OnMainMenuButtonPressed;

        _playingWindow.PauseButtonPressed -= OnPauseButtonPressed;
        _playingWindow.SwapAbilityButtonPressed -= OnSwapAbilityButtonPressed;

        _swapAbilityWindow.ReturnButtonPressed -= OnReturnButtonPressed;

        _pauseMenuWindow.MainMenuButtonPressed -= OnMainMenuButtonPressed;
        _pauseMenuWindow.ReturnButtonPressed -= OnReturnButtonPressed;
        _pauseMenuWindow.ResetLevelButtonPressed -= OnResetButtonPressed;

        _endLevelWindow.MainMenuButtonPressed -= OnMainMenuButtonPressed;
        _endLevelWindow.ResetLevelButtonPressed -= OnResetButtonPressed;
    }

    private void SubscribeToLevelButtonStorage()
    {
        _levelButtonsStorage.LevelActivated += OnLevelActivated;
    }

    private void UnsubscribeFromLevelButtonStorage()
    {
        _levelButtonsStorage.LevelActivated -= OnLevelActivated;
    }
    #endregion

    #region Windows callbacks
    private void OnHideMainMenuButtonPressed()
    {
        _game.ActivateBackgroundGame();
    }

    public void OnMainMenuButtonPressed()
    {
        _game.ActivateMainMenu();
    }

    private void OnOptionsButtonPressed()
    {
        _game.ActivateOptions();
    }

    private void OnPlayButtonPressed()
    {
        _game.PrepareLevel();
    }

    private void OnSwapAbilityButtonPressed()
    {
        _game.ActivateSwapAbility();
    }

    private void OnReturnButtonPressed()
    {
        _game.Return();
    }

    private void OnPauseButtonPressed()
    {
        _game.Pause();
    }

    private void OnResetButtonPressed()
    {
        _game.Reset();
    }
    #endregion

    private void OnLevelActivated(int indexOfLevel)
    {
        LevelSettings levelSettings = _storageLevelSettings.GetBlockFieldSettings(indexOfLevel);
        _truckFillingCardCreator.SetTruckTypeGenerator(_truckGeneratorCreator.Create(levelSettings.TruckFieldSettings.Types,
                                                                                     levelSettings.TruckFieldSettings.AmountProbabilityReduction));

        _game.PrepareLevel(_gameWorldCreator.Create(_placementSettings,
                                                    _pathSettings,
                                                    _gameWorldSettingsCreator.PrepareGameWorldSettings(levelSettings)),
                           _blockFillingCardCreator.Create(levelSettings.BlockFieldSettings),
                           _truckFillingCardCreator.Create(levelSettings.TruckFieldSettings),
                           _cartrigeBoxFillingCardCreator.Create(levelSettings.CartrigeBoxSettings));
        _game.Play();
    }

    private void OnLevelPassed()
    {

    }

    private void OnLevelFailed()
    {

    }
}