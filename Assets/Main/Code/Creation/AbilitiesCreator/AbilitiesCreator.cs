using System;
using System.Collections.Generic;

public class AbilitiesCreator
{
    private readonly IInput _input;
    private readonly CommonLevelSettings _commonLevelSettings;
    private readonly ApplicationStateStorage _applicationStateStorage;
    private readonly EventBus _eventBus;
    private readonly InputStateStorage _inputStateStorage;
    private readonly Production _production;
    private readonly PresenterPaintingSettings _presenterPaintingSettings;
    private readonly SphereCastPresenterDetector _sphereCastPresenterDetector;
    private readonly GameObjectTriggerDetector _gameObjectTriggerDetector;
    private readonly StateWindowStorage _windowsStorage;
    private readonly SaveOfPlayer _saveOfPlayer;
    private readonly LevelElementCreatorStorage _levelElementCreatorStorage;
    private readonly LevelSettingsStorage _levelSettingsStorage;
    private readonly LevelSettingsCreator _levelSettingsCreator;

    public AbilitiesCreator(IInput input,
                            CommonLevelSettings commonLevelSettings,
                            ApplicationStateStorage applicationStateStorage,
                            EventBus eventBus,
                            InputStateStorage inputStateStorage,
                            Production production,
                            PresenterPaintingSettings presenterPaintingSettings,
                            SphereCastPresenterDetector sphereCastPresenterDetector,
                            GameObjectTriggerDetector gameObjectTriggerDetector,
                            StateWindowStorage windowsStorage,
                            SaveOfPlayer saveOfPlayer,
                            LevelElementCreatorStorage levelElementCreatorStorage,
                            LevelSettingsStorage levelSettingsStorage,
                            LevelSettingsCreator levelSettingsCreator)
    {
        Validator.ValidateNotNull(input,
                                  commonLevelSettings,
                                  applicationStateStorage,
                                  eventBus,
                                  inputStateStorage,
                                  production,
                                  presenterPaintingSettings,
                                  sphereCastPresenterDetector,
                                  gameObjectTriggerDetector,
                                  windowsStorage,
                                  saveOfPlayer,
                                  levelElementCreatorStorage,
                                  levelSettingsStorage,
                                  levelSettingsCreator);

        _input = input;
        _commonLevelSettings = commonLevelSettings;
        _applicationStateStorage = applicationStateStorage;
        _eventBus = eventBus;
        _inputStateStorage = inputStateStorage;
        _production = production;
        _presenterPaintingSettings = presenterPaintingSettings;
        _sphereCastPresenterDetector = sphereCastPresenterDetector;
        _gameObjectTriggerDetector = gameObjectTriggerDetector;
        _windowsStorage = windowsStorage;
        _saveOfPlayer = saveOfPlayer;
        _levelElementCreatorStorage = levelElementCreatorStorage;
        _levelSettingsStorage = levelSettingsStorage;
        _levelSettingsCreator = levelSettingsCreator;
    }

    public List<IApplicationAbility> Create()
    {
        if (_applicationStateStorage.TryGet(out UpdateApplicationState updateApplicationState) == false)
        {
            throw new InvalidOperationException();
        }

        DeltaTimeFactorCalculator deltaTimeFactorCalculator = new DeltaTimeFactorCalculator(_input,
                                                                                            _commonLevelSettings.GlobalSettings.DeltaTimeFactorSettings);
        DeltaTimeProvider deltaTimeProvider = new DeltaTimeProvider(updateApplicationState,
                                                                    deltaTimeFactorCalculator.DeltaTimeFactor);
        TickEngine gameTickEngine = new TickEngine(_eventBus,
                                                   deltaTimeProvider.DeltaTime,
                                                   new List<Type>
                                                   {
                                                       typeof(IMover),
                                                       typeof(IRotator),
                                                       typeof(Jelly)
                                                   });
        InputStateMachine inputStateMachine = new InputStateMachine();
        WindowsSwitcher windowsSwitcher = new WindowsSwitcher(_windowsStorage,
                                                              inputStateMachine,
                                                              _saveOfPlayer.AvailableLevelsAmount);

        WindowHandlersCreator windowHandlersCreator = new WindowHandlersCreator(_windowsStorage, _inputStateStorage);
        InputStateSwitcher inputStateSwitcher = new InputStateSwitcher(updateApplicationState,
                                                                       _eventBus,
                                                                       _inputStateStorage,
                                                                       inputStateMachine,
                                                                       windowHandlersCreator.Create());

        TickEngineRegulator gameTickEngineRegulator = new TickEngineRegulator(inputStateSwitcher.InputStateMachine,
                                                                              gameTickEngine,
                                                                              new PauseConditions().GetConditions());
        TickEngine backgroundTickEngine = new TickEngine(_eventBus,
                                                         deltaTimeProvider.DeltaTime,
                                                         new List<Type>
                                                         {
                                                             typeof(WindowAnimation)
                                                         });
        SceneReloader sceneReloader = new SceneReloader(_input);
        ModelFinalizer modelFinalizer = new ModelFinalizer(_eventBus);
        PresenterPainter presenterPainter = new PresenterPainter(_presenterPaintingSettings);
        ModelPresenterBinder modelPresenterBinder = new ModelPresenterBinder(_eventBus,
                                                                             _production,
                                                                             presenterPainter);
        BlockFieldManipulator blockFieldManipulator = new BlockFieldManipulator(_eventBus,
                                                                                _commonLevelSettings.BlockFieldManipulatorSettings.AmountShiftedRows);
        
        if (_inputStateStorage.TryGet(out PlayingInputState playingInputState) == false)
        {
            throw new InvalidOperationException();
        }
        
        ModelSelector modelSelector = new ModelSelector(_eventBus,
                                                        _sphereCastPresenterDetector,
                                                        _input,
                                                        playingInputState);
        FinishedTruckDestroyer finishedTruckDestroyer = new FinishedTruckDestroyer(_gameObjectTriggerDetector);
        DelayedInvoker delayedInvoker = new DelayedInvoker(_eventBus, new ParalleledCommandBuilder(_production));
        LevelCreator levelCreator = new LevelCreator(_levelElementCreatorStorage, _eventBus, _applicationStateStorage);
        LevelSelector levelSelector = new LevelSelector(_windowsStorage, levelCreator, _levelSettingsStorage, _levelSettingsCreator, _saveOfPlayer);
        InputLogger inputLogger = new InputLogger(_input);

        return new List<IApplicationAbility>()
        {
            deltaTimeFactorCalculator,
            deltaTimeProvider,
            gameTickEngine,
            inputStateSwitcher,
            gameTickEngineRegulator,
            backgroundTickEngine,
            sceneReloader,
            modelFinalizer,
            modelPresenterBinder,
            blockFieldManipulator,
            modelSelector,
            finishedTruckDestroyer,
            delayedInvoker,
            windowsSwitcher,
            levelSelector,
            //inputLogger
        };
    }
}