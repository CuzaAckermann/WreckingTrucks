using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private WindowsStorage _windowsStorage;
    [SerializeField] private ApplicationReceiver _applicationReceiver;

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
    [SerializeField] private AnimationSettings _animationSettings;
    [SerializeField] private LevelInformerInitializer _levelInformer;

    [Header("Sound")]
    [SerializeField] private SoundInformer _soundInformer;

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

    private EventBus _eventBus;

    // CREATORS
    private LevelSettingsCreator _levelSettingsCreator;
    private KeyboardInputCreator _keyboardInputCreator;
    private ModelProductionCreator _modelProductionCreator;
    private MoverUpdaterCreator _moverCreator;
    private RotatorUpdaterCreator _rotatorCreator;
    private BlockFieldManipulatorCreator _blockFieldManipulatorCreator;

    // COMPUTER PLAYER
    private ComputerPlayerCreator _computerPlayerCreator;

    // STORAGES
    private ApplicationStateStorage _applicationStateStorage;
    private InputStateStorage _inputStateStorage;
    private LevelElementCreatorStorage _levelElementCreatorStorage;

    // TICK
    private DeltaTimeFactorCalculator _deltaTimeFactorCalculator;
    private DeltaTimeProvider _deltaTimeProvider;
    private TickEngine _gameTickEngine;
    private TickEngine _backgroundTickEngine;

    private StopwatchCreator _stopwatchCreator;
    private DelayedInvoker _delayedInvoker;

    // TICK REGULATOR
    private TickEngineRegulator _gameTickEngineRegulator;

    // LEVEL
    private LevelCreator _levelCreator;
    private LevelSelector _levelSelector;

    // STATE SWITCHER
    private InputStateSwitcher _inputStateSwitcher;

    // LEVEL ELEMENT
    private FinishedTruckDestroyer _finishedTruckDestroyer;

    // END LEVEL ELEMENT
    private EndLevelRewardCreator _endLevelRewardCreator;
    private EndLevelProcessCreator _endLevelProcessCreator;

    // APPLICATION LISTENERS
    private ApplicationAbilities _applicationAbilities;

    // ABILITY CREATORS
    private ModelFinalizerCreator _modelFinalizerCreator;
    private JellyShakerCreator _jellyShackerCreator;
    private ModelPresenterBinderCreator _modelPresenterBinderCreator;

    // ABILITIES
    private SceneReloader _sceneReloader;
    private ModelFinalizer _modelFinalizer;
    private JellyShaker _jellyShaker;
    private ModelPresenterBinder _modelPresenterBinder;
    private MoverUpdater _moverUpdater;
    private RotatorUpdater _rotatorUpdater;
    private BlockFieldManipulator _blockFieldManipulator;
    private ModelSelector _modelSelector;
    private InputLogger _inputLogger;

    #region Unity Methods
    private void Awake()
    {
        InitAll();
    }

    private void OnEnable()
    {
        _applicationReceiver.Prepare();
    }

    private void Start()
    {
        _applicationReceiver.Launch();
    }

    private void OnDisable()
    {
        _applicationReceiver.Stop();
    }

    private void OnDestroy()
    {
        _applicationReceiver.Finish();
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

    #region Initialization
    private void InitAll()
    {
        ConfigureApplication();

        //
        InitEventBus();

        InitCreators();

        InitStorages();

        InitApplicationReceiver();

        InitTick();

        InitWindowStorage();

        InitLevelCreator();

        InitLevelSelector();

        InitInputStateSwitcher();

        InitApplicationListeners();

        InitInformers();

        InitEndLevelElements();
        //

        InitTestAbilities();
    }

    private void InitEventBus()
    {
        _eventBus = new EventBus();
    }

    private void InitCreators()
    {
        _commonLevelSettings.SetFieldTransforms(_fieldTransforms);

        _levelSettingsCreator = new LevelSettingsCreator(_commonLevelSettings);

        _keyboardInputCreator = new KeyboardInputCreator(_keyboardInputSettings);

        _modelProductionCreator = new ModelProductionCreator(_modelFactoriesSettings,
                                                             _modelsSettings,
                                                             new TrunkCreator(_modelFactoriesSettings.TrunkFactorySettings,
                                                                              _modelsSettings.TrunkSettings),
                                                             _eventBus);

        _presenterProductionCreator.Initialize(_eventBus);
        _modelFinalizerCreator = new ModelFinalizerCreator();
        _modelPresenterBinderCreator = new ModelPresenterBinderCreator(_presenterProductionCreator, _presenterPainter);
        _jellyShackerCreator = new JellyShakerCreator(_eventBus, _presenterProductionCreator.GetPresenterProduction());
        _moverCreator = new MoverUpdaterCreator(_commonLevelSettings.GlobalSettings.MoverSettings);
        _rotatorCreator = new RotatorUpdaterCreator(_commonLevelSettings.GlobalSettings.RotatorSettings);
        _blockFieldManipulatorCreator = new BlockFieldManipulatorCreator(_commonLevelSettings.BlockFieldManipulatorSettings, _eventBus);

        _computerPlayerCreator = new ComputerPlayerCreator(_commonLevelSettings.ComputerPlayerSettings,
                                                           _eventBus);
        //
    }

    private void InitStorages()
    {
        _applicationStateStorage = new ApplicationStateStorage();
        _inputStateStorage = new InputStateStorage(_keyboardInputCreator.GetKeyboardInput());
        _levelElementCreatorStorage = new LevelElementCreatorStorage(_commonLevelSettings,
                                                                     _eventBus,
                                                                     _modelProductionCreator,
                                                                     _presenterProductionCreator,
                                                                     _additionalRoadSettings);
    }

    private void InitApplicationReceiver()
    {
        _applicationReceiver.Init(_applicationStateStorage);
    }

    private void InitTick()
    {
        _deltaTimeFactorCalculator = new DeltaTimeFactorCalculator(_keyboardInputCreator.GetKeyboardInput(),
                                                                   _commonLevelSettings.GlobalSettings.DeltaTimeFactorSettings);
        _deltaTimeProvider = new DeltaTimeProvider(_applicationReceiver.ApplicationStateStorage.UpdateApplicationState,
                                                   _deltaTimeFactorCalculator.DeltaTimeFactor);
        _gameTickEngine = new TickEngine(_applicationStateStorage, _deltaTimeProvider.DeltaTime);
        _backgroundTickEngine = new TickEngine(_applicationStateStorage, _deltaTimeProvider.DeltaTime);

        _stopwatchCreator = new StopwatchCreator(_modelFactoriesSettings.StopwatchFactorySettings);
        _delayedInvoker = new DelayedInvoker(_applicationStateStorage, _eventBus, new ParalleledCommandBuilder(_stopwatchCreator));

        _gameTickEngine.AddTickableCreator(_stopwatchCreator);
        _gameTickEngine.AddTickableCreator(_moverCreator);
        _gameTickEngine.AddTickableCreator(_rotatorCreator);
        _gameTickEngine.AddTickableCreator(_jellyShackerCreator);
    }

    private void InitWindowStorage()
    {
        _windowsStorage.Init(_inputStateStorage,
                             _animationSettings,
                             _backgroundTickEngine,
                             _saveOfPlayer.AvailableLevelsAmount);
    }

    private void InitLevelCreator()
    {
        _levelCreator = new LevelCreator(_levelElementCreatorStorage,
                                         _eventBus,
                                         _applicationStateStorage);
    }

    private void InitLevelSelector()
    {
        _levelSelector = new LevelSelector(_windowsStorage,
                                           _levelCreator,
                                           _storageLevelSettings,
                                           _levelSettingsCreator,
                                           _saveOfPlayer);
    }

    private void InitInputStateSwitcher()
    {
        _inputStateSwitcher = new InputStateSwitcher(_applicationStateStorage,
                                                     _eventBus,
                                                     _windowsStorage,
                                                     _inputStateStorage);
    }

    private void InitApplicationListeners()
    {
        _gameTickEngineRegulator = new TickEngineRegulator(_inputStateSwitcher.InputStateMachine, _gameTickEngine);
        _sceneReloader = new SceneReloader(_keyboardInputCreator.GetKeyboardInput());
        _modelFinalizer = _modelFinalizerCreator.Create(_eventBus);
        _jellyShaker = _jellyShackerCreator.Create();
        _modelPresenterBinder = _modelPresenterBinderCreator.Create(_eventBus);
        _moverUpdater = _moverCreator.Create(_eventBus);
        _rotatorUpdater = _rotatorCreator.Create(_eventBus);
        _blockFieldManipulator = _blockFieldManipulatorCreator.Create();
        _modelSelector = new ModelSelector(_eventBus, _presenterDetector, _keyboardInputCreator.GetKeyboardInput(), _inputStateStorage.PlayingInputState);
        _finishedTruckDestroyer = new FinishedTruckDestroyer(_triggerDetectorForFinishedTruck);
        _inputLogger = new InputLogger(_keyboardInputCreator.GetKeyboardInput());

        //

        _applicationAbilities = new ApplicationAbilities(_applicationStateStorage, new List<IAbility>
        {
            _deltaTimeFactorCalculator,
            _deltaTimeProvider,
            _gameTickEngineRegulator,
            _sceneReloader,
            _modelFinalizer,
            _jellyShaker,
            _modelPresenterBinder,
            _moverUpdater,
            _rotatorUpdater,
            _blockFieldManipulator,
            _modelSelector,
            _finishedTruckDestroyer,
            _inputLogger
        });
    }

    private void InitInformers()
    {
        _gameTickEngine.AddTickableCreator(_levelInformer.BlockFieldInformer);

        _levelInformer.Init(_eventBus);
        _soundInformer.Init(_eventBus);
    }

    private void InitEndLevelElements()
    {
        _endLevelRewardCreator = new EndLevelRewardCreator(_eventBus, _uIPositionDeterminator);
        _endLevelProcessCreator = new EndLevelProcessCreator(_endLevelRewardCreator);
    }
    #endregion

    #region Test Abilities
    private void InitTestAbilities()
    {
        _testerAbilities.Init(_stopwatchCreator.Create(),
                              _deltaTimeFactorCalculator.DeltaTimeFactor,
                              _eventBus,
                              _keyboardInputCreator.GetKeyboardInput());
    }
    #endregion
}