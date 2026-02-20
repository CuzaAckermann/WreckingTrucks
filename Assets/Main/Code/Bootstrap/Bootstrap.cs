using System;
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
    [SerializeField] private PresenterPaintingSettings _paintingSettings;

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
    private ComputerPlayerCreator _computerPlayerCreator;
    private AbilitiesCreator _abilitiesCreator;

    private Production _production;

    // STORAGES
    private ApplicationStateStorage _applicationStateStorage;
    private InputStateStorage _inputStateStorage;
    private LevelElementCreatorStorage _levelElementCreatorStorage;

    // TICK
    private DelayedInvoker _delayedInvoker;

    // LEVEL
    private LevelCreator _levelCreator;
    private LevelSelector _levelSelector;

    // END LEVEL ELEMENT
    private EndLevelRewardCreator _endLevelRewardCreator;
    private EndLevelProcessCreator _endLevelProcessCreator;

    // APPLICATION ABILITIES
    private ApplicationAbilities _applicationAbilities;

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

        InitAbilitiesCreator();

        InitApplicationReceiver();

        InitTick();

        InitWindowStorage();

        InitLevelCreator();

        InitLevelSelector();

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

    private void InitAbilitiesCreator()
    {
        _abilitiesCreator = new AbilitiesCreator(_keyboardInputCreator.GetKeyboardInput(),
                                                 _commonLevelSettings,
                                                 _applicationStateStorage,
                                                 _eventBus,
                                                 _windowsStorage,
                                                 _inputStateStorage,
                                                 _production,
                                                 _paintingSettings,
                                                 _presenterDetector,
                                                 _triggerDetectorForFinishedTruck);
    }

    private void InitApplicationReceiver()
    {
        _applicationReceiver.Init(_applicationStateStorage);
    }

    private void InitTick()
    {
        _delayedInvoker = new DelayedInvoker(_applicationStateStorage, _eventBus, new ParalleledCommandBuilder(_production));
    }

    private void InitWindowStorage()
    {
        _windowsStorage.Init(_inputStateStorage,
                             _animationSettings,
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

    private void InitApplicationListeners()
    {
        _applicationAbilities = new ApplicationAbilities(_applicationStateStorage, _abilitiesCreator.Create());
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
                              //_deltaTimeFactorCalculator.DeltaTimeFactor,
                              _eventBus,
                              _keyboardInputCreator.GetKeyboardInput());
    }
    #endregion
}