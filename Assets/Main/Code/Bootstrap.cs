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

    [Header("Settings")]
    [Header("Game World Settings")]
    [SerializeField] private PlacementSettings _placementSettings;
    [SerializeField] private PathSettings _pathSettings;
    [SerializeField] private GameWorldSettings _gameWorldSettings;
    [SerializeField] private StorageLevelSettings _storageLevelSettings;

    [Header("Game World Elements Creator Settings")]
    [SerializeField] private ModelFactoriesSettings _modelFactoriesSettings;
    [SerializeField] private FillerCreatorSettings _fillerCreatorSettings;
    [SerializeField] private PresenterProductionCreator _presenterProductionCreator;
    [SerializeField] private PositionCorrector _positionCorrector;

    [Header("Input Settings")]
    [SerializeField] private KeyboardInputSettings _keyboardInputSettings;

    [Header("Detectors")]
    [SerializeField] private TruckPresenterDetector _truckPresenterDetector;
    [SerializeField] private BlockPresenterDetector _blockPresenterDetector;

    [Header("Application Configurator")]
    [SerializeField] private ApplicationConfigurator _applicationConfigurator;
    [SerializeField] private DeltaTimeCalculator _deltaTimeCalculator;

    // MAIN

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

    // JOINT CREATORS

    private BinderCreator _binderCreator;
    private ModelFinalizerCreator _modelFinalizerCreator;
    private StopwatchCreator _stopwatchCreator;

    private MoverCreator _moverCreator;
    private FillerCreator _fillerCreator;
    private RotatorCreator _rotatorCreator;

    private LayerCreator _layerCreator;
    private ColumnCreator _columnCreator;

    // BLOCK

    private BlockFieldCreator _blockFieldCreator;

    // TRUCK

    private TruckFieldCreator _truckFieldCreator;
    private TruckGeneratorCreator _truckGeneratorCreator;

    // CARTRIGEBOX

    private CartrigeBoxFieldCreator _cartrigeBoxFieldCreator;

    // ROAD

    private PathCreator _pathCreator;
    private RoadCreator _roadCreator;

    // SHOOTING

    private BulletSimulationCreator _bulletSimulationCreator;
    private ChargerCreator _chargerCreator;

    // SUPPLIER

    private SupplierCreator _supplierCreator;

    // COMPUTER PLAYER

    private BackgroundGameCreator _backgroundGameCreator;
    private ComputerPlayerCreator _computerPlayerCreator;
    private TypesCalculatorCreator _typesCalculatorCreator;

    // SWAP ABILITY

    private SwapAbilityCreator _swapAbilityCreator;
    private BlockFieldManipulatorCreator _blockFieldManipulatorCreator;

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

        _positionCorrector.CorrectTransformable(_placementSettings.TruckField,
                                                _gameWorldSettings.TruckSpaceSettings.FieldSettings.FieldSize,
                                                _gameWorldSettings.TruckSpaceSettings.FieldIntervals);

        _applicationConfigurator.ConfigureApplication();

        // корректировка

        InitializeConverters();
        InitializeTimeElements();
        InitializeSettingsCreators();
        InitializeProductionCreators();
        InitializeFillingCardCreators();
        InitializeSpaceElementCreators();
        InitializeSpaceCreators();
        InitailizeMainCreators();
        InitializeGameState();
        BindStateToWindow();
        InitializeGame();
    }

    private void OnEnable()
    {
        SubscribeToWindows();
    }

    private void Start()
    {
        HideAllWindows();

        OnMainMenuButtonPressed();
    }

    private void Update()
    {
        _game.Update(_deltaTimeCalculator.GetDeltaTime());
    }

    private void OnDisable()
    {
        UnsubscribeFromWindows();
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

    private void InitializeTimeElements()
    {
        _deltaTimeCalculator = new DeltaTimeCalculator();
        _tickEngine = new TickEngine();
        _stopwatchCreator = new StopwatchCreator(_tickEngine);
    }

    private void InitializeSettingsCreators()
    {
        _gameWorldSettingsCreator = new GameWorldSettingsCreator(_gameWorldSettings);
    }

    private void InitializeProductionCreators()
    {
        _modelProductionCreator = new ModelProductionCreator(_modelFactoriesSettings,
                                                             new TrunkCreator(),
                                                             new BlockTrackerCreator(_gameWorldSettings.BlockTrackerCreatorSettings.AcceptableAngle),
                                                             _stopwatchCreator);
        
        _presenterProductionCreator.Initialize();
    }

    private void InitializeFillingCardCreators()
    {
        _blockFillingCardCreator = new BlockFillingCardCreator(_modelProductionCreator.CreateBlockProduction(),
                                                               _blockTypeConverter);
        _truckFillingCardCreator = new TruckFillingCardCreator(_modelProductionCreator.CreateTruckProduction(),
                                                               _truckTypeConverter);
        _cartrigeBoxFillingCardCreator = new CartrigeBoxFillingCardCreator(_modelProductionCreator.CreateCartrigeBoxProduction(),
                                                                           _cartrigeBoxTypeConverter);
    }

    private void InitializeSpaceElementCreators()
    {
        _binderCreator = new BinderCreator(_presenterProductionCreator);
        _modelFinalizerCreator = new ModelFinalizerCreator();

        _moverCreator = new MoverCreator(_tickEngine);
        _fillerCreator = new FillerCreator(_fillerCreatorSettings, _stopwatchCreator);
        _rotatorCreator = new RotatorCreator(_tickEngine);

        _columnCreator = new ColumnCreator();
        _layerCreator = new LayerCreator(_columnCreator);
        
        _blockFieldCreator = new BlockFieldCreator(_layerCreator);
        _truckFieldCreator = new TruckFieldCreator(_layerCreator);
        _cartrigeBoxFieldCreator = new CartrigeBoxFieldCreator(_layerCreator);
        InitializeRoadCreator();
        InitializeBulletSimulationCreator();
        _supplierCreator = new SupplierCreator();

        _truckGeneratorCreator = new TruckGeneratorCreator(new CreatedTypesCreator<TruckTypeConverter>(_truckTypeConverter));
        
        InitializeBackgroundGameCreator();
        InitializeSwapAbility();
    }

    private void InitializeRoadCreator()
    {
        _pathCreator = new PathCreator();
        _roadCreator = new RoadCreator(_pathCreator);
    }

    private void InitializeBulletSimulationCreator()
    {
        _chargerCreator = new ChargerCreator(_modelProductionCreator.CreateBulletFactory());
        _bulletSimulationCreator = new BulletSimulationCreator(_chargerCreator);
    }

    private void InitializeBackgroundGameCreator()
    {
        _typesCalculatorCreator = new TypesCalculatorCreator();
        _computerPlayerCreator = new ComputerPlayerCreator(_gameWorldSettings.ComputerPlayerSettings,
                                                           _stopwatchCreator,
                                                           _typesCalculatorCreator);
        _backgroundGameCreator = new BackgroundGameCreator(_computerPlayerCreator);
    }

    private void InitializeSwapAbility()
    {
        _swapAbilityCreator = new SwapAbilityCreator();
        _blockFieldManipulatorCreator = new BlockFieldManipulatorCreator();
    }

    private void InitializeSpaceCreators()
    {
        _blockSpaceCreator = new BlockSpaceCreator(_blockFieldCreator,
                                                   _moverCreator,
                                                   _fillerCreator,
                                                   _blockFillingCardCreator);
        _truckSpaceCreator = new TruckSpaceCreator(_truckFieldCreator,
                                                   _moverCreator,
                                                   _fillerCreator,
                                                   _modelProductionCreator,
                                                   _truckGeneratorCreator,
                                                   _truckFillingCardCreator);
        _cartrigeBoxSpaceCreator = new CartrigeBoxSpaceCreator(_cartrigeBoxFieldCreator,
                                                               _moverCreator,
                                                               _fillerCreator,
                                                               _cartrigeBoxFillingCardCreator);
        _roadSpaceCreator = new RoadSpaceCreator(_roadCreator,
                                                 _moverCreator,
                                                 _rotatorCreator);
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
                                                 _binderCreator,
                                                 _modelFinalizerCreator);

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

        // корректировка

        SwapAbility swapAbility = _swapAbilityCreator.Create();

        _swapAbilityState = new SwapAbilityState(_blockPresenterDetector,
                                                 _keyboardInputHandlerCreator.CreateSwapAbilityInputHandler(),
                                                 _blockFieldManipulatorCreator.Create(_gameWorldSettings.BlockFieldManipulatorSettings),
                                                 swapAbility,
                                                 _moverCreator.Create(swapAbility, _gameWorldSettings.SwapAbilitySettings.MoverSettings));

        // корректировка

        _pausedState = new PausedState(_keyboardInputHandlerCreator.CreatePauseInputHandler());
        _endLevelState = new EndLevelState();
    }

    private void BindStateToWindow()
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
        _game = new Game(_gameWorldCreator,
                         _tickEngine,
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

        _levelButtonsStorage.ReturnButtonPressed += OnReturnButtonPressed;
        _levelButtonsStorage.LevelActivated += OnLevelActivated;

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

        _levelButtonsStorage.ReturnButtonPressed -= OnReturnButtonPressed;
        _levelButtonsStorage.LevelActivated -= OnLevelActivated;

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
    #endregion

    #region Windows callbacks
    private void OnHideMainMenuButtonPressed()
    {
        _game.ShowFullScreenBackgroundGame();
    }

    public void OnMainMenuButtonPressed()
    {
        _game.ActivateMainMenu();
    }

    private void OnLevelActivated(int indexOfLevel)
    {
        _game.BuildLevel(_gameWorldSettingsCreator.PrepareGameWorldSettings(_placementSettings,
                                                                            _pathSettings,
                                                                            _storageLevelSettings.GetLevelSettings(indexOfLevel)));
    }

    private void OnOptionsButtonPressed()
    {
        _game.ActivateOptions();
    }

    private void OnPlayButtonPressed()
    {
        _game.ActivateLevelSelection();
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
}