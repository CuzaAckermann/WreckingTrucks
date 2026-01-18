using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [Header("Settings")]
    [Header("Global Settings")]
    [SerializeField] private GameWorldSettings _gameWorldSettings;
    [SerializeField] private StorageLevelSettings _storageLevelSettings;

    [Header("Model Creator Settings")]
    [SerializeField] private ModelFactoriesSettings _modelFactoriesSettings;
    [SerializeField] private ModelsSettings _modelsSettings;

    [Header("Game World Element Settings")]
    [SerializeField] private PlacementSettings _fieldTransforms;
    [SerializeField] private BezierCurveSettings _additionalRoadSettings;

    [Header("Input Settings")]
    [SerializeField] private KeyboardInputSettings _keyboardInputSettings;

    [Space(20)]
    [Header("Presenters")]
    [Header("Production")]
    [SerializeField] private PresenterProductionCreator _presenterProductionCreator;

    [Header("Indication")]
    [Header("Visual")]
    [SerializeField] private GameWorldInformer _gameWorldInformer;

    [Header("Sound")]
    [SerializeField] private ShootingSoundPlayer _shootingSoundPlayer;

    [Header("Painter")]
    [SerializeField] private PresenterPainter _presenterPainter;

    [Space(20)]
    [Header("Detectors")]
    [Header("Sphere Cast Detectors")]
    [SerializeField] private TruckPresenterDetector _truckPresenterDetector;
    [SerializeField] private BlockPresenterDetector _blockPresenterDetector;
    [SerializeField] private PlanePresenterDetector _planePresenterDetector;

    [Header("On Trigger Detector")]
    [SerializeField] private GameObjectTriggerDetector _triggerDetector;

    [Space(20)]
    [Header("Application Configurator")]
    [SerializeField] private ApplicationConfigurator _applicationConfigurator;
    [SerializeField] private PositionCorrector _positionCorrector;
    [SerializeField] private UIPositionDeterminator _uIPositionDeterminator;

    [Space(20)]
    [Header("Tester Abilities")]
    [SerializeField] private TesterAbilities _testerAbilities;

    // SETTINGS CREATORS
    private GameWorldSettingsCreator _gameWorldSettingsCreator;

    // TICK ENGINE AND EVENT BUS
    private TickEngine _tickEngine;
    private EventBus _eventBus;

    // TIME ELEMENT CREATOR
    private StopwatchCreator _stopwatchCreator;

    // FIELD ELEMENTS CREATOR
    private ColumnCreator _columnCreator;
    private LayerCreator _layerCreator;

    private BlockFieldCreator _blockFieldCreator;
    private TruckFieldCreator _truckFieldCreator;
    private CartrigeBoxFieldCreator _cartrigeBoxFieldCreator;

    // PRODUCTION CREATOR
    private ModelProductionCreator _modelProductionCreator;

    // POSITION CHANGER CREATORS
    private MoverCreator _moverCreator;
    private RotatorCreator _rotatorCreator;

    // EFFECTS CREATORS
    private JellyShakerCreator _jellyShackerCreator;

    // DESTROYER CREATORS
    private FinishedTruckDestroyer _finishedTruckDestroyer;
    private ModelFinalizerCreator _modelFinalizerCreator;

    // BIND CREATORS
    private ModelPresenterBinderCreator _modelPresenterBinderCreator;

    // GENERATORS CREATOR
    private RowGeneratorCreator _rowGeneratorCreator;
    private ModelColorGeneratorCreator _truckGeneratorCreator;

    // FILLING CARD CREATORS (RECORD STORAGES)
    private BlockFillingCardCreator _blockFillingCardCreator;
    private CartrigeBoxFillingCardCreator _cartrigeBoxFillingCardCreator;

    // FILLING
    private RecordStorageCreator _recordStorageCreator;

    private FillingStrategiesCreator _fillingStrategiesCreator;

    private BlockFillerCreator _blockFillerCreator;
    private TruckFillerCreator _truckFillerCreator;
    private CartrigeBoxFillerCreator _cartrigeBoxFillerCreator;

    // OTHER GAME WORLD
    private RoadCreator _roadCreator;
    private PlaneSlotCreator _planeSlotCreator;

    // COMPUTER PLAYER
    private BackgroundGameCreator _backgroundGameCreator;
    private ComputerPlayerCreator _computerPlayerCreator;
    private TypesCalculatorCreator _typesCalculatorCreator;

    // DISPENCER CREATOR
    private DispencerCreator _dispencerCreator;

    // GAME WORLD CREATOR
    private GameWorldCreator _gameWorldCreator;

    // INPUT CREATOR
    private KeyboardInputHandlerCreator _keyboardInputHandlerCreator;

    // BACKGROUND INPUT
    private SceneReseter _sceneReseter;
    private DeltaTimeCoefficientDefiner _deltaTimeCoefficientDefiner;

    // STATES
    private BackgroundGameState _backgroundGameState;
    private MainMenuState _mainMenuState;
    private LevelSelectionState _levelSelectionState;
    private ShopState _shopState;
    private OptionsMenuState _optionsMenuState;
    private PlayingState _playingState;
    private PausedState _pausedState;
    private EndLevelState _endLevelState;

    // GAME
    private Game _game;

    // NOT WORK
    private SwapAbilityCreator _swapAbilityCreator;
    private BlockFieldManipulatorCreator _blockFieldManipulatorCreator;

    private EndLevelRewardCreator _endLevelRewardCreator;
    private EndLevelProcessCreator _endLevelProcessCreator;

    private SwapAbilityState _swapAbilityState;

    private void Awake()
    {
        ConfigureApplication();

        InitSettingsCreators();

        InitTickEngineAndEventBus();
        InitStopwatchCreator();

        InitFieldCreators();
        InitProductionCreators();

        InitTickableCreators();

        InitGlobalEntities();

        InitGenerations();
        InitFillingCardCreators();
        InitGameWorldCreators();

        InitBackgroundGameCreator();
        InitSwapAbility();
        InitEndLevelProcess();

        InitDispencerCreator();

        InitGameWorldCreator();
        InitGameWorldToInformerBinder();

        InitInputs();

        InitGameState();
        BindStateToWindow();

        InitGame();
    }

    private void OnEnable()
    {
        SubscribeToInputs();
        SubscribeToWindows();
        SubscribeToGame();
    }

    private void Start()
    {
        HideAllWindows();

        InitTestAbilities();

        _game.Start();
    }

    private void Update()
    {
        _sceneReseter.Update();
        _deltaTimeCoefficientDefiner.Update();

        _game.Update(Time.deltaTime * _deltaTimeCoefficientDefiner.CurrentAmount);
    }

    private void OnDisable()
    {
        UnsubscribeFromInputs();
        UnsubscribeFromWindows();
        UnsubscribeFromGame();
    }

    #region Configuring game
    private void ConfigureApplication()
    {
        _positionCorrector.CorrectTransformable(_fieldTransforms.TruckFieldTransform,
                                                _gameWorldSettings.GlobalSettings.TruckFieldSize,
                                                _gameWorldSettings.GlobalSettings.TruckFieldIntervals);

        _applicationConfigurator.ConfigureApplication();
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
    #endregion

    #region Initializations
    private void InitSettingsCreators()
    {
        _gameWorldSettings.SetFieldTransforms(_fieldTransforms);

        _gameWorldSettingsCreator = new GameWorldSettingsCreator(_gameWorldSettings);
    }

    private void InitTickEngineAndEventBus()
    {
        _tickEngine = new TickEngine();
        _eventBus = new EventBus();
    }

    private void InitStopwatchCreator()
    {
        _stopwatchCreator = new StopwatchCreator(_modelFactoriesSettings.StopwatchFactorySettings);
        _tickEngine.AddTickableCreator(_stopwatchCreator);
    }

    private void InitFieldCreators()
    {
        _columnCreator = new ColumnCreator();
        _layerCreator = new LayerCreator(_columnCreator);

        _blockFieldCreator = new BlockFieldCreator(_layerCreator);
        _truckFieldCreator = new TruckFieldCreator(_layerCreator);
        _cartrigeBoxFieldCreator = new CartrigeBoxFieldCreator(_layerCreator);
    }

    private void InitProductionCreators()
    {
        _modelProductionCreator = new ModelProductionCreator(_modelFactoriesSettings,
                                                             _modelsSettings,
                                                             new TrunkCreator(),
                                                             _stopwatchCreator,
                                                             new BlockTrackerCreator(_eventBus),
                                                             _eventBus);

        _presenterProductionCreator.Initialize(_eventBus);
    }

    private void InitTickableCreators()
    {
        _moverCreator = new MoverCreator(_gameWorldSettings.GlobalSettings.MoverSettings);
        _rotatorCreator = new RotatorCreator(_gameWorldSettings.GlobalSettings.RotatorSettings);
        _jellyShackerCreator = new JellyShakerCreator();

        _tickEngine.AddTickableCreator(_moverCreator);
        _tickEngine.AddTickableCreator(_rotatorCreator);
        _tickEngine.AddTickableCreator(_jellyShackerCreator);
    }

    private void InitGlobalEntities()
    {
        _finishedTruckDestroyer = new FinishedTruckDestroyer(_triggerDetector);
        _modelFinalizerCreator = new ModelFinalizerCreator();
        _modelPresenterBinderCreator = new ModelPresenterBinderCreator(_presenterProductionCreator, _presenterPainter);
        _shootingSoundPlayer.Initialize(_eventBus);

        // Необходимо для того чтобы сущности работали
        _modelFinalizerCreator.Create(_eventBus);
        _jellyShackerCreator.Create(_eventBus);
        _modelPresenterBinderCreator.Create(_eventBus);
        //_shootingSoundPlayer;
        _moverCreator.Create(_eventBus);
        _rotatorCreator.Create(_eventBus);
    }

    private void InitGenerations()
    {
        _rowGeneratorCreator = new RowGeneratorCreator(_modelProductionCreator,
                                                       new List<ColorType>(_gameWorldSettings.NonstopGameSettings.GeneratedColorTypes));

        _truckGeneratorCreator = new ModelColorGeneratorCreator(_gameWorldSettings.GlobalSettings.ModelTypeGeneratorSettings);
    }

    private void InitFillingCardCreators()
    {
        _blockFillingCardCreator = new BlockFillingCardCreator();
        _cartrigeBoxFillingCardCreator = new CartrigeBoxFillingCardCreator();
    }

    private void InitGameWorldCreators()
    {
        _recordStorageCreator = new RecordStorageCreator(_blockFillingCardCreator, _rowGeneratorCreator);

        _fillingStrategiesCreator = new FillingStrategiesCreator(_modelProductionCreator,
                                                                 _stopwatchCreator,
                                                                 _presenterProductionCreator.CreateSpawnDetectorFactory(),
                                                                 _gameWorldSettings.GlobalSettings.FillerSettings);

        _blockFillerCreator = new BlockFillerCreator(_fillingStrategiesCreator);

        _truckFillerCreator = new TruckFillerCreator(_fillingStrategiesCreator,
                                                     _truckGeneratorCreator);

        _cartrigeBoxFillerCreator = new CartrigeBoxFillerCreator(_fillingStrategiesCreator);
        
        _roadCreator = new RoadCreator(_additionalRoadSettings);
        _planeSlotCreator = new PlaneSlotCreator(_modelProductionCreator.CreateFactory<Plane>());
    }

    private void InitBackgroundGameCreator()
    {
        _typesCalculatorCreator = new TypesCalculatorCreator();
        _computerPlayerCreator = new ComputerPlayerCreator(_gameWorldSettings.ComputerPlayerSettings,
                                                           _stopwatchCreator,
                                                           _typesCalculatorCreator,
                                                           _eventBus);
        _backgroundGameCreator = new BackgroundGameCreator(_computerPlayerCreator);
    }

    private void InitSwapAbility()
    {
        _swapAbilityCreator = new SwapAbilityCreator();
        _blockFieldManipulatorCreator = new BlockFieldManipulatorCreator();
    }

    private void InitEndLevelProcess()
    {
        _endLevelRewardCreator = new EndLevelRewardCreator(_stopwatchCreator, _uIPositionDeterminator);
        _endLevelProcessCreator = new EndLevelProcessCreator(_endLevelRewardCreator);
    }

    private void InitDispencerCreator()
    {
        _dispencerCreator = new DispencerCreator();
    }

    private void InitGameWorldCreator()
    {
        _gameWorldCreator = new GameWorldCreator(_blockFieldCreator, _truckFieldCreator, _cartrigeBoxFieldCreator,
                                                 _recordStorageCreator,
                                                 _blockFillerCreator, _truckFillerCreator, _cartrigeBoxFillerCreator,
                                                 _roadCreator,
                                                 _planeSlotCreator,
                                                 _dispencerCreator,
                                                 _gameWorldSettingsCreator,
                                                 _storageLevelSettings,
                                                 _eventBus);
    }

    private void InitGameWorldToInformerBinder()
    {
        _tickEngine.AddTickableCreator(_gameWorldInformer);

        _gameWorldInformer.Init(_eventBus);
    }

    private void InitInputs()
    {
        _keyboardInputHandlerCreator = new KeyboardInputHandlerCreator(_keyboardInputSettings);
        _sceneReseter = _keyboardInputHandlerCreator.CreateSceneReseter();
        _deltaTimeCoefficientDefiner = _keyboardInputHandlerCreator.CreateDeltaTimeCoefficientDefiner();
    }

    private void InitGameState()
    {
        _backgroundGameState = new BackgroundGameState();
        _mainMenuState = new MainMenuState();
        _levelSelectionState = new LevelSelectionState(_tickEngine);
        _optionsMenuState = new OptionsMenuState();
        _shopState = new ShopState();
        _playingState = new PlayingState(_eventBus,
                                         _truckPresenterDetector,
                                         _blockPresenterDetector,
                                         _planePresenterDetector,
                                         _keyboardInputHandlerCreator.CreatePlayingInputHandler());

        // корректировка

        SwapAbility swapAbility = _swapAbilityCreator.Create();

        _swapAbilityState = new SwapAbilityState(_blockPresenterDetector,
                                                 _keyboardInputHandlerCreator.CreateSwapAbilityInputHandler(),
                                                 _blockFieldManipulatorCreator.Create(_gameWorldSettings.BlockFieldManipulatorSettings),
                                                 swapAbility,
                                                 _eventBus);

        // корректировка

        _pausedState = new PausedState(_keyboardInputHandlerCreator.CreatePauseInputHandler(), _tickEngine);
        _endLevelState = new EndLevelState(_eventBus, _endLevelProcessCreator.Create());
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

    private void InitGame()
    {
        _game = new Game(_eventBus,
                         _gameWorldCreator,
                         _tickEngine,
                         _backgroundGameCreator,
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
    #endregion

    #region Test Abilities
    private void InitTestAbilities()
    {
        _testerAbilities.Init(_stopwatchCreator,
                              _deltaTimeCoefficientDefiner,
                              _eventBus);
    }
    #endregion

    #region Input Handlers
    private void SubscribeToInputs()
    {
        _sceneReseter.ResetSceneButtonPressed += OnResetSceneButtonPressed;
    }

    private void UnsubscribeFromInputs()
    {
        _sceneReseter.ResetSceneButtonPressed -= OnResetSceneButtonPressed;
    }

    private void OnResetSceneButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        _levelButtonsStorage.LevelActivated -= OnLevelActivated;
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

    #region Game Subscribes / Unsubscribes
    private void SubscribeToGame()
    {
        _eventBus.Subscribe<LevelPassedSignal>(OnLevelPassed);
        _eventBus.Subscribe<LevelFailedSignal>(OnLevelFailed);
    }

    private void UnsubscribeFromGame()
    {
        _eventBus.Unsubscribe<LevelPassedSignal>(OnLevelPassed);
        _eventBus.Unsubscribe<LevelFailedSignal>(OnLevelFailed);
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

    private void OnNonstopGameButtonPressed()
    {
        _game.ActivateNonstopGame();
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
    private void OnLevelPassed(LevelPassedSignal _)
    {
        // корректировка Здесь кнопка следующего уровня доступна
        _endLevelWindow.SetLevelNavigationState(_game.HasNextLevel, _game.HasPreviousLevel);
    }

    private void OnLevelFailed(LevelFailedSignal _)
    {
        // корректировка Здесь кнопка следующего уровня не доступна
        _endLevelWindow.SetLevelNavigationState(_game.HasNextLevel, _game.HasPreviousLevel);
    }
    #endregion
}