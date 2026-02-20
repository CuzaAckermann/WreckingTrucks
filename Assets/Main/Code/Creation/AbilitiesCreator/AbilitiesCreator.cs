using System;
using System.Collections.Generic;

public class AbilitiesCreator
{
    private readonly IInput _input;
    private readonly CommonLevelSettings _commonLevelSettings;
    private readonly ApplicationStateStorage _applicationStateStorage;
    private readonly EventBus _eventBus;
    private readonly IWindowsStorage _windowsStorage;
    private readonly InputStateStorage _inputStateStorage;
    private readonly Production _production;
    private readonly PresenterPaintingSettings _presenterPaintingSettings;
    private readonly SphereCastPresenterDetector _sphereCastPresenterDetector;
    private readonly GameObjectTriggerDetector _gameObjectTriggerDetector;

    public AbilitiesCreator(IInput input,
                            CommonLevelSettings commonLevelSettings,
                            ApplicationStateStorage applicationStateStorage,
                            EventBus eventBus,
                            IWindowsStorage windowsStorage,
                            InputStateStorage inputStateStorage,
                            Production production,
                            PresenterPaintingSettings presenterPaintingSettings,
                            SphereCastPresenterDetector sphereCastPresenterDetector,
                            GameObjectTriggerDetector gameObjectTriggerDetector)
    {
        Validator.ValidateNotNull(input,
                                  commonLevelSettings,
                                  applicationStateStorage,
                                  eventBus,
                                  windowsStorage,
                                  inputStateStorage,
                                  production,
                                  presenterPaintingSettings,
                                  sphereCastPresenterDetector,
                                  gameObjectTriggerDetector);

        _input = input;
        _commonLevelSettings = commonLevelSettings;
        _applicationStateStorage = applicationStateStorage;
        _eventBus = eventBus;
        _windowsStorage = windowsStorage;
        _inputStateStorage = inputStateStorage;
        _production = production;
        _presenterPaintingSettings = presenterPaintingSettings;
        _sphereCastPresenterDetector = sphereCastPresenterDetector;
        _gameObjectTriggerDetector = gameObjectTriggerDetector;
    }

    public List<IAbility> Create()
    {
        DeltaTimeFactorCalculator deltaTimeFactorCalculator = new DeltaTimeFactorCalculator(_input,
                                                                                            _commonLevelSettings.GlobalSettings.DeltaTimeFactorSettings);
        DeltaTimeProvider deltaTimeProvider = new DeltaTimeProvider(_applicationStateStorage.UpdateApplicationState,
                                                                    deltaTimeFactorCalculator.DeltaTimeFactor);
        TickEngine gameTickEngine = new TickEngine(_eventBus,
                                                   deltaTimeProvider.DeltaTime,
                                                   new List<Type>
                                                   {
                                                       typeof(IMover),
                                                       typeof(IRotator),
                                                       typeof(Jelly)
                                                   });
        InputStateSwitcher inputStateSwitcher = new InputStateSwitcher(_applicationStateStorage,
                                                                       _eventBus,
                                                                       _windowsStorage,
                                                                       _inputStateStorage);

        TickEngineRegulator gameTickEngineRegulator = new TickEngineRegulator(inputStateSwitcher.InputStateMachine,
                                                                              gameTickEngine);
        TickEngine backgroundTickEngine = new TickEngine(_eventBus,
                                                         deltaTimeProvider.DeltaTime,
                                                         new List<Type>
                                                         {
                                                             typeof(WindowAnimation)
                                                         });
        SceneReloader sceneReloader = new SceneReloader(_input);
        ModelFinalizer modelFinalizer = new ModelFinalizer(_eventBus);
        PresenterPainter presenterPainter = new PresenterPainter(_presenterPaintingSettings);
        ModelPresenterBinder modelPresenterBinder = new ModelPresenterBinder(_eventBus, _production, presenterPainter);
        BlockFieldManipulator blockFieldManipulator = new BlockFieldManipulator(_eventBus, _commonLevelSettings.BlockFieldManipulatorSettings.AmountShiftedRows);
        ModelSelector modelSelector = new ModelSelector(_eventBus, _sphereCastPresenterDetector, _input, _inputStateStorage.PlayingInputState);
        FinishedTruckDestroyer finishedTruckDestroyer = new FinishedTruckDestroyer(_gameObjectTriggerDetector);
        InputLogger inputLogger = new InputLogger(_input);

        return new List<IAbility>()
        {
            deltaTimeFactorCalculator,
            deltaTimeProvider,
            gameTickEngine,
            gameTickEngineRegulator,
            backgroundTickEngine,
            sceneReloader,
            modelFinalizer,
            modelPresenterBinder,
            blockFieldManipulator,
            modelSelector,
            finishedTruckDestroyer,
            inputLogger
        };
    }
}