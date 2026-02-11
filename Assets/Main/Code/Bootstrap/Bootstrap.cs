using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private WindowsStorage _windowsStorage;
    [SerializeField] private UpdateEmitter _updateEmitter;
    [SerializeField] private ApplicationStateStorage _applicationStateStorage;

    [Space(20)]
    [Header("Settings")]
    [Header("Global Settings")]
    [SerializeField] private CommonLevelSettings _commonLevelSettings;
    [SerializeField] private LevelSettingsStorage _storageLevelSettings;

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
    [SerializeField] private LevelInformerInitializer _levelInformer;
    [SerializeField] private AnimationSettings _animationSettings;

    [Header("Sound")]
    [SerializeField] private ShootingSoundPlayer _shootingSoundPlayer;

    [Header("Painter")]
    [SerializeField] private PresenterPainter _presenterPainter;

    [Space(20)]
    [Header("Detectors")]
    [Header("Sphere Cast Detectors")]
    [SerializeField] private SphereCastPresenterDetector _presenterDetector;

    [Header("On Trigger Detector")]
    [SerializeField] private GameObjectTriggerDetector _triggerDetectorForFinishedTruck;

    [Space(20)]
    [Header("Application Configurator")]
    [SerializeField] private SaveOfPlayer _saveOfPlayer;
    [SerializeField] private ApplicationConfigurator _applicationConfigurator;
    [SerializeField] private PositionCorrector _positionCorrector;
    [SerializeField] private UIPositionDeterminator _uIPositionDeterminator;

    [Space(20)]
    [Header("Tester Abilities")]
    [SerializeField] private TesterAbilities _testerAbilities;

    // SETTINGS CREATORS
    private LevelSettingsCreator _levelSettingsCreator;

    // TICK ENGINE AND EVENT BUS
    private TickEngine _gameTickEngine;
    private TickEngine _backgroundTickEngine;
    private EventBus _eventBus;

    // TIME ELEMENT CREATOR
    private StopwatchCreator _stopwatchCreator;

    // DELAYED INVOKER
    private DelayedInvoker _delayedInvoker;

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
    private RotatorUpdaterCreator _rotatorCreator;

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
    private ModelSlotCreator _planeSlotCreator;
    private ModelPlacerCreator _modelPlacerCreator;

    // COMPUTER PLAYER
    private BackgroundGameCreator _backgroundGameCreator;
    private ComputerPlayerCreator _computerPlayerCreator;
    private TypesCalculatorCreator _typesCalculatorCreator;

    // DISPENCER CREATOR
    private DispencerCreator _dispencerCreator;

    // LEVEL CREATION
    private LevelSelector _levelSelector;
    private LevelCreator _levelCreator;

    // INPUT CREATOR
    private KeyboardInputCreator _keyboardInputHandlerCreator;

    // BACKGROUND INPUT
    private BackgroundInput _backgroundInput;
    private SceneReloader _sceneReloader;
    private DeltaTimeFactorDefiner _deltaTimeFactorDefiner;

    // STATES
    private StateStorage _stateStorage;

    private BackgroundGameState _backgroundGameState;
    private MainMenuInputState _mainMenuState;
    private GameSelectionState _gameSelectionState;
    private LevelSelectionState _levelSelectionState;
    private ShopState _shopState;
    private OptionsMenuState _optionsMenuState;
    private PlayingInputState _playingState;
    private PausedState _pausedState;
    private EndLevelState _endLevelState;

    private InputStateSwitcher _inputStateSwitcher;

    // SELECTOR
    private ModelSelector _modelSelector;

    // GAME_SIGNAL_EMITTER
    private GameSignalEmitter _gameSignalEmitter;

    // NOT WORK
    private BlockFieldManipulatorCreator _blockFieldManipulatorCreator;

    private EndLevelRewardCreator _endLevelRewardCreator;
    private EndLevelProcessCreator _endLevelProcessCreator;

    private SwapAbilityState _swapAbilityState;

    #region Unity callbacks
    private void Awake()
    {
        InitAll();
    }

    private void Start()
    {
        _gameSignalEmitter.Start();
    }

    private void OnDisable()
    {
        _gameSignalEmitter.Stop();
    }

    private void OnDestroy()
    {
        _gameSignalEmitter.Clear();
    }
    #endregion

    #region Configuring game
    private void ConfigureApplication()
    {
        _positionCorrector.CorrectTransformable(_fieldTransforms.TruckFieldTransform,
                                                _commonLevelSettings.GlobalSettings.TruckFieldSize,
                                                _commonLevelSettings.GlobalSettings.TruckFieldIntervals);

        _applicationConfigurator.ConfigureApplication();
    }
    #endregion

    #region Initializations
    private void InitAll()
    {
        ConfigureApplication();

        InitSettingsCreators();

        InitTickEngineAndEventBus();
        InitStopwatchCreator();

        InitDelayedInvoker();

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

        InitLevelCreator();
        InitGameWorldToInformerBinder();

        InitInputs();

        InitGameState();

        InitMain();

        InitLevelSelector();

        InitTestAbilities();
    }

    private void InitSettingsCreators()
    {
        _commonLevelSettings.SetFieldTransforms(_fieldTransforms);

        _levelSettingsCreator = new LevelSettingsCreator(_commonLevelSettings);
    }

    private void InitTickEngineAndEventBus()
    {
        _eventBus = new EventBus();
        _gameTickEngine = new TickEngine(_eventBus);
        _backgroundTickEngine = new TickEngine(_eventBus);
    }

    private void InitStopwatchCreator()
    {
        _stopwatchCreator = new StopwatchCreator(_modelFactoriesSettings.StopwatchFactorySettings);
        _gameTickEngine.AddTickableCreator(_stopwatchCreator);
    }

    private void InitDelayedInvoker()
    {
        _delayedInvoker = new DelayedInvoker(_eventBus, new ParalleledCommandBuilder(_stopwatchCreator));
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
                                                             new TrunkCreator(_modelFactoriesSettings.TrunkFactorySettings,
                                                                              _modelsSettings.TrunkSettings),
                                                             _eventBus);

        _presenterProductionCreator.Initialize(_eventBus);
    }

    private void InitTickableCreators()
    {
        _moverCreator = new MoverCreator(_commonLevelSettings.GlobalSettings.MoverSettings);
        _rotatorCreator = new RotatorUpdaterCreator(_commonLevelSettings.GlobalSettings.RotatorSettings);
        _jellyShackerCreator = new JellyShakerCreator();

        _gameTickEngine.AddTickableCreator(_moverCreator);
        _gameTickEngine.AddTickableCreator(_rotatorCreator);
        _gameTickEngine.AddTickableCreator(_jellyShackerCreator);
    }

    private void InitGlobalEntities()
    {
        _finishedTruckDestroyer = new FinishedTruckDestroyer(_triggerDetectorForFinishedTruck);
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
                                                       new List<ColorType>(_commonLevelSettings.NonstopGameSettings.GeneratedColorTypes));

        _truckGeneratorCreator = new ModelColorGeneratorCreator(_commonLevelSettings.GlobalSettings.ModelTypeGeneratorSettings);
    }

    private void InitFillingCardCreators()
    {
        _blockFillingCardCreator = new BlockFillingCardCreator();
        _cartrigeBoxFillingCardCreator = new CartrigeBoxFillingCardCreator();
    }

    private void InitGameWorldCreators()
    {
        _recordStorageCreator = new RecordStorageCreator(_rowGeneratorCreator);

        _fillingStrategiesCreator = new FillingStrategiesCreator(_eventBus,
                                                                 _modelProductionCreator,
                                                                 _presenterProductionCreator.CreateSpawnDetectorFactory(),
                                                                 _commonLevelSettings.GlobalSettings.FillerSettings);

        _blockFillerCreator = new BlockFillerCreator(_fillingStrategiesCreator);

        _truckFillerCreator = new TruckFillerCreator(_fillingStrategiesCreator,
                                                     _truckGeneratorCreator);

        _cartrigeBoxFillerCreator = new CartrigeBoxFillerCreator(_fillingStrategiesCreator);
        
        _roadCreator = new RoadCreator(_additionalRoadSettings);
        _planeSlotCreator = new ModelSlotCreator();
        _modelPlacerCreator = new ModelPlacerCreator(_modelProductionCreator);
    }

    private void InitBackgroundGameCreator()
    {
        _typesCalculatorCreator = new TypesCalculatorCreator();
        _computerPlayerCreator = new ComputerPlayerCreator(_commonLevelSettings.ComputerPlayerSettings,
                                                           _typesCalculatorCreator,
                                                           _eventBus);
        _backgroundGameCreator = new BackgroundGameCreator(_computerPlayerCreator);
    }

    private void InitSwapAbility()
    {
        _blockFieldManipulatorCreator = new BlockFieldManipulatorCreator();
    }

    private void InitEndLevelProcess()
    {
        _endLevelRewardCreator = new EndLevelRewardCreator(_eventBus, _uIPositionDeterminator);
        _endLevelProcessCreator = new EndLevelProcessCreator(_endLevelRewardCreator);
    }

    private void InitDispencerCreator()
    {
        _dispencerCreator = new DispencerCreator();
    }

    private void InitLevelCreator()
    {
        _levelCreator = new LevelCreator(_blockFieldCreator, _truckFieldCreator, _cartrigeBoxFieldCreator,
                                         _blockFillingCardCreator, _recordStorageCreator,
                                         _blockFillerCreator, _truckFillerCreator, _cartrigeBoxFillerCreator,
                                         _roadCreator,
                                         _planeSlotCreator, _modelPlacerCreator,
                                         _dispencerCreator,
                                         _eventBus);
    }

    private void InitGameWorldToInformerBinder()
    {
        _gameTickEngine.AddTickableCreator(_levelInformer.BlockFieldInformer);

        _levelInformer.Init(_eventBus);
    }

    private void InitInputs()
    {
        _keyboardInputHandlerCreator = new KeyboardInputCreator(_eventBus, _keyboardInputSettings);
        _backgroundInput = _keyboardInputHandlerCreator.CreateBackgroundInput();

        // Необходимо для того чтобы сущности работали
        _sceneReloader = new SceneReloader(_backgroundInput);
        _deltaTimeFactorDefiner = new DeltaTimeFactorDefiner(_backgroundInput,
                                                             _commonLevelSettings.GlobalSettings.DeltaTimeFactorSettings);
    }

    private void InitGameState()
    {
        _backgroundGameState = new BackgroundGameState();
        _mainMenuState = new MainMenuInputState();
        _gameSelectionState = new GameSelectionState();
        _levelSelectionState = new LevelSelectionState(_gameTickEngine);
        _optionsMenuState = new OptionsMenuState();
        _shopState = new ShopState();

        PlayingInput playingInput = _keyboardInputHandlerCreator.CreatePlayingInput();
        _modelSelector = new ModelSelector(_eventBus, _presenterDetector, playingInput);
        _playingState = new PlayingInputState(playingInput);

        // корректировка

        _swapAbilityState = new SwapAbilityState(_presenterDetector,
                                                 _keyboardInputHandlerCreator.CreateSwapAbilityInput(),
                                                 _blockFieldManipulatorCreator.Create(_eventBus, _commonLevelSettings.BlockFieldManipulatorSettings),
                                                 _eventBus);

        // корректировка

        _pausedState = new PausedState(_keyboardInputHandlerCreator.CreatePauseInput(), _gameTickEngine);
        _endLevelState = new EndLevelState(_eventBus, _endLevelProcessCreator.Create());

        _stateStorage = new StateStorage(_backgroundGameState,
                                         _mainMenuState,
                                         _gameSelectionState,
                                         _levelSelectionState,
                                         _optionsMenuState,
                                         _shopState,
                                         _playingState,
                                         _swapAbilityState,
                                         _pausedState,
                                         _endLevelState);

        _inputStateSwitcher = new InputStateSwitcher(_eventBus,
                                                     _windowsStorage,
                                                     _stateStorage);
    }

    private void InitMain()
    {
        _gameSignalEmitter = new GameSignalEmitter(_eventBus);

        _windowsStorage.Init(_eventBus,
                             _stateStorage,
                             _animationSettings,
                             _backgroundTickEngine,
                             _saveOfPlayer.AvailableLevelsAmount);

        _updateEmitter.Init(_eventBus, _deltaTimeFactorDefiner.DeltaTimeFactor);
        _applicationStateStorage.Init();
    }

    private void InitLevelSelector()
    {
        _levelSelector = new LevelSelector(_windowsStorage,
                                           _levelCreator,
                                           _storageLevelSettings,
                                           _levelSettingsCreator,
                                           _saveOfPlayer);
    }
    #endregion

    #region Test Abilities
    private void InitTestAbilities()
    {
        _testerAbilities.Init(_stopwatchCreator.Create(),
                              _deltaTimeFactorDefiner.DeltaTimeFactor,
                              _eventBus,
                              _backgroundInput);
    }
    #endregion
}