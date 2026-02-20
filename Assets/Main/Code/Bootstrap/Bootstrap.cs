using System;
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

    [Header("Presenter Creator Settings")]
    [SerializeField] private PresenterFactoriesSettings _presenterFactorySettings;
    [SerializeField] private Transform _poolParent;

    [Header("Game World Element Settings")]
    [SerializeField] private PlacementSettings _fieldTransforms;
    [SerializeField] private BezierCurveSettings _additionalRoadSettings;

    [Header("Input Settings")]
    [SerializeField] private KeyboardInputSettings _keyboardInputSettings;

    [Space(20)]
    [Header("Presenters")]
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

    private ProductionCreator _productionCreator;
    private Production _production;

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
    private ModelPresenterBinderCreator _modelPresenterBinderCreator;

    // ABILITIES
    private SceneReloader _sceneReloader;
    private ModelFinalizer _modelFinalizer;
    private ModelPresenterBinder _modelPresenterBinder;

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

        _productionCreator = new ProductionCreator(_eventBus,
                                                        _modelFactoriesSettings,
                                                        _modelsSettings,
                                                        _presenterFactorySettings,
                                                        _poolParent,
                                                        Instantiate,
                                                        _commonLevelSettings);

        _production = _productionCreator.GetProduction();

        _modelFinalizerCreator = new ModelFinalizerCreator();
        _modelPresenterBinderCreator = new ModelPresenterBinderCreator(_presenterPainter, _production);
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
                                                                     _production,
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
        _gameTickEngine = new TickEngine(_applicationStateStorage,
                                         _eventBus,
                                         _deltaTimeProvider.DeltaTime,
                                         new List<Type>
                                         {
                                             typeof(IMover),
                                             typeof(IRotator),
                                             typeof(Jelly)
                                         });
        _backgroundTickEngine = new TickEngine(_applicationStateStorage,
                                               _eventBus,
                                               _deltaTimeProvider.DeltaTime,
                                               new List<Type>
                                               {
                                                   typeof(WindowAnimation)
                                               });

        _delayedInvoker = new DelayedInvoker(_applicationStateStorage, _eventBus, new ParalleledCommandBuilder(_production));
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
        _modelPresenterBinder = _modelPresenterBinderCreator.Create(_eventBus);

        _blockFieldManipulator = _blockFieldManipulatorCreator.Create();
        _modelSelector = new ModelSelector(_eventBus, _presenterDetector, _keyboardInputCreator.GetKeyboardInput(), _inputStateStorage.PlayingInputState);
        _finishedTruckDestroyer = new FinishedTruckDestroyer(_triggerDetectorForFinishedTruck);
        //_inputLogger = new InputLogger(_keyboardInputCreator.GetKeyboardInput());

        //

        _applicationAbilities = new ApplicationAbilities(_applicationStateStorage, new List<IAbility>
        {
            _deltaTimeFactorCalculator,
            _deltaTimeProvider,
            _gameTickEngineRegulator,
            _sceneReloader,
            _modelFinalizer,
            _modelPresenterBinder,
            _blockFieldManipulator,
            _modelSelector,
            _finishedTruckDestroyer,
            //_inputLogger
        });
    }

    private void InitInformers()
    {
        if (_production.TryCreate(out SmoothValueFollower smoothValueFollower) == false)
        {
            throw new InvalidOperationException();
        }

        _levelInformer.Init(_eventBus, smoothValueFollower);
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
        if (_production.TryCreate(out Stopwatch stopwatch) == false)
        {
            throw new InvalidOperationException();
        }

        _testerAbilities.Init(stopwatch,
                              _deltaTimeFactorCalculator.DeltaTimeFactor,
                              _eventBus,
                              _keyboardInputCreator.GetKeyboardInput());
    }
    #endregion
}