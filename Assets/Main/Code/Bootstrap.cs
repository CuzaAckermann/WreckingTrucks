using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [Header("Windows")]
    [SerializeField] private BackgroundGameWindow _backgroundGameWindow;
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private LevelButtonsStorage _levelButtonsStorage;
    [SerializeField] private OptionsMenu _optionsMenu;
    [SerializeField] private ShopWindow _shopWindow;
    [SerializeField] private PlayingWindow _playingWindow;
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private EndLevelWindow _endLevelWindow;
    [SerializeField] private SwapAbilityWindow _swapAbilityWindow;

    [Space(20)]
    [Header("Presenters")]
    [SerializeField] private PresenterProductionCreator _presenterProductionCreator;

    [Space(20)]
    [Header("Settings")]
    [Header("Game World Settings")]
    [SerializeField] private GameWorldSettings _gameWorldSettings;
    [SerializeField] private StorageLevelSettings _storageLevelSettings;
    [SerializeField] private PlacementSettings _fieldPositions;

    [Header("Road Settings")]
    [SerializeField] private BezierCurveSettings _additionalRoadSettings;

    [Header("Game World Elements Creator Settings")]
    [SerializeField] private ModelFactoriesSettings _modelFactoriesSettings;
    [SerializeField] private FillerCreatorSettings _fillerCreatorSettings;

    [Header("Input Settings")]
    [SerializeField] private KeyboardInputSettings _keyboardInputSettings;

    [Space(20)]
    [Header("Detectors")]
    [SerializeField] private TruckPresenterDetector _truckPresenterDetector;
    [SerializeField] private BlockPresenterDetector _blockPresenterDetector;
    [SerializeField] private PlanePresenterDetector _planePresenterDetector;

    [Space(20)]
    [Header("Indications")]
    [SerializeField] private GameWorldToInformerBinder _gameWorldToInformerBinder;

    [Space(20)]
    [Header("Application Configurator")]
    [SerializeField] private ApplicationConfigurator _applicationConfigurator;
    [SerializeField] private DeltaTimeCoefficientChanger _deltaTimeCoefficientChanger;
    [SerializeField] private PositionCorrector _positionCorrector;
    [SerializeField] private UIPositionDeterminator _uIPositionDeterminator;

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
    private PlaneSpaceCreator _planeSpaceCreator;
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
    private ShopState _shopState;
    private OptionsMenuState _optionsMenuState;
    private PlayingState _playingState;
    private PausedState _pausedState;
    private EndLevelState _endLevelState;

    // ABILITY STATES
    private SwapAbilityState _swapAbilityState;

    // END LEVEL
    private EndLevelRewardCreator _endLevelRewardCreator;
    private EndLevelProcessCreator _endLevelProcessCreator;

    private void Awake()
    {
        ConfigureApplication();

        InitializeConverters();
        InitializeTimeElements();
        InitializeSettingsCreators();
        InitializeProductionCreators();
        InitializeFillingCardCreators();
        InitializeSpaceElementCreators();
        InitializeEndLevelProcess();
        InitializeSpaceCreators();
        InitailizeMainCreators();
        InitializeGameState();
        BindStateToWindow();
        InitializeGame();
        InitializeGameWorldToInformerBinder();
    }

    private void OnEnable()
    {
        SubscribeToWindows();
        SubscribeToGame();
    }

    private void Start()
    {
        HideAllWindows();
        _game.Start();
    }

    private void Update()
    {
        _game.Update(Time.deltaTime * _deltaTimeCoefficientChanger.DeltaTimeCoefficient);
    }

    private void OnDisable()
    {
        UnsubscribeFromWindows();
        UnsubscribeFromGame();
    }

    #region Configuring game
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

    private void ConfigureApplication()
    {
        _deltaTimeCoefficientChanger.Initialize();
        _positionCorrector.CorrectTransformable(_fieldPositions.TruckFieldPosition,
                                                _gameWorldSettings.TruckSpaceSettings);

        _applicationConfigurator.ConfigureApplication();
    }
    #endregion

    #region Initializations
    private void InitializeConverters()
    {
        _blockTypeConverter = new BlockTypeConverter();
        _truckTypeConverter = new TruckTypeConverter();
        _cartrigeBoxTypeConverter = new CartrigeBoxTypeConverter();
    }

    private void InitializeTimeElements()
    {
        _tickEngine = new TickEngine();
        _stopwatchCreator = new StopwatchCreator(_tickEngine);
    }

    private void InitializeSettingsCreators()
    {
        _gameWorldSettingsCreator = new GameWorldSettingsCreator(_gameWorldSettings,
                                                                 _fieldPositions);
    }

    private void InitializeProductionCreators()
    {
        _modelProductionCreator = new ModelProductionCreator(_modelFactoriesSettings,
                                                             new TrunkCreator(),
                                                             new BlockTrackerCreator(_gameWorldSettings.BlockTrackerCreatorSettings),
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
        _roadCreator = new RoadCreator(_additionalRoadSettings);
        _chargerCreator = new ChargerCreator(_modelProductionCreator.CreateBulletFactory());
        _bulletSimulationCreator = new BulletSimulationCreator(_chargerCreator);
        _supplierCreator = new SupplierCreator();

        _truckGeneratorCreator = new TruckGeneratorCreator(new CreatedTypesCreator<TruckTypeConverter>(_truckTypeConverter));

        InitializeBackgroundGameCreator();
        InitializeSwapAbility();
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

    private void InitializeEndLevelProcess()
    {
        _endLevelRewardCreator = new EndLevelRewardCreator(_stopwatchCreator, _uIPositionDeterminator);
        _endLevelProcessCreator = new EndLevelProcessCreator(_endLevelRewardCreator,
                                                             _moverCreator,
                                                             _modelFinalizerCreator);
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
        _planeSpaceCreator = new PlaneSpaceCreator(_moverCreator,
                                                   _rotatorCreator,
                                                   _roadCreator,
                                                   _modelProductionCreator.CreatePlaneFactory());
        _roadSpaceCreator = new RoadSpaceCreator(_roadCreator,
                                                 _moverCreator,
                                                 _rotatorCreator);
        _shootingSpaceCreator = new ShootingSpaceCreator(_bulletSimulationCreator,
                                                         _moverCreator,
                                                         _rotatorCreator);
        _supplierSpaceCreator = new SupplierSpaceCreator(_supplierCreator,
                                                         _moverCreator,
                                                         _rotatorCreator);
    }

    private void InitailizeMainCreators()
    {
        _gameWorldCreator = new GameWorldCreator(_blockSpaceCreator,
                                                 _truckSpaceCreator,
                                                 _cartrigeBoxSpaceCreator,
                                                 _planeSpaceCreator,
                                                 _roadSpaceCreator,
                                                 _shootingSpaceCreator,
                                                 _supplierSpaceCreator,
                                                 _binderCreator,
                                                 _modelFinalizerCreator,
                                                 _gameWorldSettingsCreator,
                                                 _storageLevelSettings);

        _keyboardInputHandlerCreator = new KeyboardInputHandlerCreator(_keyboardInputSettings);
    }

    private void InitializeGameState()
    {
        _backgroundGameState = new BackgroundGameState();
        _mainMenuState = new MainMenuState();
        _levelSelectionState = new LevelSelectionState();
        _optionsMenuState = new OptionsMenuState();
        _shopState = new ShopState();
        _playingState = new PlayingState(_truckPresenterDetector,
                                         _planePresenterDetector,
                                         _keyboardInputHandlerCreator.CreatePlayingInputHandler());

        // корректировка

        SwapAbility swapAbility = _swapAbilityCreator.Create();

        _swapAbilityState = new SwapAbilityState(_blockPresenterDetector,
                                                 _keyboardInputHandlerCreator.CreateSwapAbilityInputHandler(),
                                                 _blockFieldManipulatorCreator.Create(_gameWorldSettings.BlockFieldManipulatorSettings),
                                                 swapAbility,
                                                 _moverCreator.Create(swapAbility, _gameWorldSettings.SwapAbilitySettings.MoverSettings));

        // корректировка

        _pausedState = new PausedState(_keyboardInputHandlerCreator.CreatePauseInputHandler(), _tickEngine);
        _endLevelState = new EndLevelState(_endLevelProcessCreator.Create());
    }

    private void BindStateToWindow()
    {
        _backgroundGameWindow.Initialize(_backgroundGameState);
        _mainMenu.Initialize(_mainMenuState);
        _levelButtonsStorage.Initailize(_levelSelectionState, _storageLevelSettings.AmountLevels);
        _optionsMenu.Initialize(_optionsMenuState);
        _shopWindow.Initialize(_shopState);
        _playingWindow.Initialize(_playingState);
        _swapAbilityWindow.Initialize(_swapAbilityState);
        _pauseMenu.Initialize(_pausedState);
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
                         _shopState,
                         _playingState,
                         _swapAbilityState,
                         _pausedState,
                         _endLevelState);
    }

    private void InitializeGameWorldToInformerBinder()
    {
        _gameWorldToInformerBinder.Initialize(_game);
    }
    #endregion

    #region UI Subscribes / Unsubscribes
    private void SubscribeToWindows()
    {
        _backgroundGameWindow.ShowMainMenuButtonPressed += OnMainMenuButtonPressed;

        _mainMenu.HideMenuButtonPressed += OnHideMainMenuButtonPressed;
        _mainMenu.PlayButtonPressed += OnPlayButtonPressed;
        _mainMenu.OptionsButtonPressed += OnOptionsButtonPressed;
        _mainMenu.ShopButtonPressed += OnShopButtonPressed;

        _levelButtonsStorage.ReturnButtonPressed += OnReturnButtonPressed;
        _levelButtonsStorage.LevelActivated += OnLevelActivated;

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
        _levelButtonsStorage.LevelActivated -= OnLevelActivated;

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

    #region Game Subscribes / Unsubscribes
    private void SubscribeToGame()
    {
        _game.LevelPassed += OnLevelPassed;
        _game.LevelFailed += OnLevelFailed;
    }

    private void UnsubscribeFromGame()
    {
        _game.LevelPassed -= OnLevelPassed;
        _game.LevelFailed -= OnLevelFailed;
    }
    #endregion

    #region Windows callbacks
    private void OnHideMainMenuButtonPressed()
    {
        _game.ShowBackgroundGame();
    }

    public void OnMainMenuButtonPressed()
    {
        _game.OpenMainMenu();
    }

    private void OnLevelActivated(int indexOfLevel)
    {
        _game.BuildLevel(indexOfLevel);
    }

    private void OnOptionsButtonPressed()
    {
        _game.OpenOptions();
    }

    private void OnShopButtonPressed()
    {
        _game.OpenShop();
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

    private void OnPreviousLevelPressed()
    {
        _game.PlayPreviousLevel();
    }

    private void OnNextLevelPressed()
    {
        _game.PlayNextLevel();
    }
    #endregion

    #region Game Event Handlers
    private void OnLevelPassed()
    {
        // корректировка Здесь кнопка следующего уровня доступна
        _endLevelWindow.SetLevelNavigationState(_game.HasNextLevel, _game.HasPreviousLevel);
    }

    private void OnLevelFailed()
    {
        // корректировка Здесь кнопка следующего уровня не доступна
        _endLevelWindow.SetLevelNavigationState(_game.HasNextLevel, _game.HasPreviousLevel);
    }
    #endregion
}