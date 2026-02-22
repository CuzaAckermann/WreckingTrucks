using System;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private List<StateWindowBase> _stateWindows;
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
    private InputStateCreator _inputStateCreator;
    private ProductionCreator _productionCreator;
    private ComputerPlayerCreator _computerPlayerCreator;
    private AbilitiesCreator _abilitiesCreator;
    private ApplicationStatesCreator _applicationStatesCreator;

    private Production _production;

    // STORAGES
    private ApplicationStateStorage _applicationStateStorage;
    private InputStateStorage _inputStateStorage;
    private StateWindowStorage _stateWindowStorage;
    private LevelElementCreatorStorage _levelElementCreatorStorage;

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
        ValidateIncomingData();

        InitEventBus();

        InitCreators();

        InitStorages();

        InitAbilitiesCreator();

        InitApplicationReceiver();

        InitApplicationListeners();

        InitInformers();

        InitEndLevelElements();
        //

        InitTestAbilities();
    }

    private void ValidateIncomingData()
    {
        // Сделать валидации других хранилищ
        Validator.ValidateAllDerivedTypesPresent(_stateWindows);
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

        _inputStateCreator = new InputStateCreator(_keyboardInputCreator.GetKeyboardInput());

        _productionCreator = new ProductionCreator(_eventBus,
                                                   _modelFactoriesSettings,
                                                   _modelsSettings,
                                                   _presenterFactorySettings,
                                                   _poolParent,
                                                   Instantiate,
                                                   _commonLevelSettings);

        _applicationStatesCreator = new ApplicationStatesCreator();

        _production = _productionCreator.GetProduction();

        _computerPlayerCreator = new ComputerPlayerCreator(_commonLevelSettings.ComputerPlayerSettings,
                                                           _eventBus);
    }

    private void InitStorages()
    {
        _applicationStateStorage = new ApplicationStateStorage(_applicationStatesCreator.Create());
        _inputStateStorage = new InputStateStorage(_inputStateCreator.Create());
        _stateWindowStorage = new StateWindowStorage(_stateWindows);
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
                                                 _inputStateStorage,
                                                 _production,
                                                 _paintingSettings,
                                                 _presenterDetector,
                                                 _triggerDetectorForFinishedTruck,
                                                 _stateWindowStorage,
                                                 _saveOfPlayer,
                                                 _levelElementCreatorStorage,
                                                 _storageLevelSettings,
                                                 _levelSettingsCreator);
    }

    private void InitApplicationReceiver()
    {
        _applicationReceiver.Init(_applicationStateStorage);
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