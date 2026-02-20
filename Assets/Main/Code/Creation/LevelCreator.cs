public class LevelCreator
{
    private readonly LevelElementCreatorStorage _creatorStorage;
    private readonly EventBus _eventBus;
    private readonly ApplicationStateStorage _applicationStateStorage;

    //private FieldSize _blockFieldSize;
    private int _amountCartrigeBoxes;

    public LevelCreator(LevelElementCreatorStorage creatorStorage,
                        EventBus eventBus,
                        ApplicationStateStorage applicationStateStorage)
    {
        Validator.ValidateNotNull(creatorStorage,
                                  eventBus,
                                  applicationStateStorage);

        _creatorStorage = creatorStorage;
        _eventBus = eventBus;
        _applicationStateStorage = applicationStateStorage;
    }

    public Level CreateLevel(CommonLevelSettings commonLevelSettings)
    {
        _creatorStorage.BlockFillingCardCreator.SetBlockFieldSettings(commonLevelSettings.LevelSettings.BlockFieldSettings);
        //_recordStorageCreator.SetBlockFieldSettings(gameWorldSettings.LevelSettings.BlockFieldSettings);
        //_blockFieldSize = commonLevelSettings.LevelSettings.BlockFieldSettings.FieldSize;
        _amountCartrigeBoxes = commonLevelSettings.LevelSettings.AmountCartrigeBoxes;

        BlockTracker blockTracker = new BlockTracker(_eventBus);

        BlockField blockField = _creatorStorage.BlockFieldCreator.Create(commonLevelSettings.GlobalSettings.BlockFieldTransform,
                                                          commonLevelSettings.LevelSettings.BlockFieldSettings.FieldSize,
                                                          commonLevelSettings.GlobalSettings.BlockFieldIntervals,
                                                          _eventBus);

        BlockFieldFiller blockFieldFiller = _creatorStorage.BlockFillerCreator.Create(blockField,
                                                                       _creatorStorage.BlockFillingCardCreator.Create(commonLevelSettings.LevelSettings.BlockFieldSettings.FieldSize),
                                                                       _eventBus);

        Level level = CreateCommonLevel(commonLevelSettings,
                                        blockField,
                                        blockFieldFiller,
                                        blockTracker);

        return level;
    }

    public Level CreateNonstopLevel(CommonLevelSettings commonLevelSettings)
    {
        //_blockFieldSize = commonLevelSettings.NonstopGameSettings.BlockFieldSize;
        _amountCartrigeBoxes = commonLevelSettings.NonstopGameSettings.AmountCartrigeBoxes;

        BlockTracker blockTracker = new BlockTracker(_eventBus);

        BlockField blockField = _creatorStorage.BlockFieldCreator.Create(commonLevelSettings.GlobalSettings.BlockFieldTransform,
                                                          commonLevelSettings.NonstopGameSettings.BlockFieldSize,
                                                          commonLevelSettings.GlobalSettings.BlockFieldIntervals,
                                                          _eventBus);

        BlockFieldFiller blockFieldFiller = _creatorStorage.BlockFillerCreator.CreateNonstop(blockField,
                                                                              _creatorStorage.RecordStorageCreator.Create(commonLevelSettings.NonstopGameSettings.BlockFieldSize),
                                                                              commonLevelSettings.NonstopGameSettings.Frequency,
                                                                              _eventBus);

        Level gameWorld = CreateCommonLevel(commonLevelSettings,
                                                    blockField,
                                                    blockFieldFiller,
                                                    blockTracker);

        return gameWorld;
    }

    private Level CreateCommonLevel(CommonLevelSettings gameWorldSettings,
                                    BlockField blockField,
                                    BlockFieldFiller blockFieldFiller,
                                    BlockTracker blockTracker)
    {
        TruckField truckField = _creatorStorage.TruckFieldCreator.Create(gameWorldSettings.GlobalSettings.TruckFieldTransform,
                                                          gameWorldSettings.GlobalSettings.TruckFieldSize,
                                                          gameWorldSettings.GlobalSettings.TruckFieldIntervals,
                                                          _eventBus);
        CartrigeBoxField cartrigeBoxField = _creatorStorage.CartrigeBoxFieldCreator.Create(gameWorldSettings.GlobalSettings.CartrigeBoxFieldTransform,
                                                                            gameWorldSettings.GlobalSettings.CartrigeBoxFieldSize,
                                                                            gameWorldSettings.GlobalSettings.CartrigeBoxFieldIntervals,
                                                                            _eventBus);

        TruckFieldFiller truckFiller = _creatorStorage.TruckFillerCreator.Create(truckField,
                                                                  blockFieldFiller.GetUniqueStoredColors(),
                                                                  _eventBus);

        Dispencer dispencer = _creatorStorage.DispencerCreator.Create(cartrigeBoxField, _amountCartrigeBoxes, _eventBus);

        CartrigeBoxFieldFiller cartrigeBoxFieldFiller = _creatorStorage.CartrigeBoxFillerCreator.Create(cartrigeBoxField,
                                                                                         dispencer,
                                                                                         _eventBus);

        Road roadForTrucks = _creatorStorage.RoadCreator.Create(gameWorldSettings.RoadSpaceSettings.PathForTrucks);

        roadForTrucks.Prepare(truckField);

        ModelSlot planeSlot = _creatorStorage.ModelSlotCreator.Create(gameWorldSettings.PlaneSpaceSettings, _eventBus);
        ModelPlacer<Plane> planePlacer = _creatorStorage.ModelPlacerCreator.Create<Plane>(planeSlot);

        planePlacer.PlaceModel();

        TruckActivator truckActivator = new TruckActivator(_eventBus,
                                                           truckField,
                                                           dispencer,
                                                           roadForTrucks,
                                                           blockTracker);
        PlaneActivator planeActivator = new PlaneActivator(_eventBus,
                                                           blockField,
                                                           dispencer,
                                                           _creatorStorage.RoadCreator.Create(gameWorldSettings.PlaneSpaceSettings.PathForPlane),
                                                           planeSlot);
        SelectedModelDestroyer<Block> selectedBlockDestroyer = new SelectedModelDestroyer<Block>(_applicationStateStorage, _eventBus);

        EndLevelDefiner endLevelDefiner = new EndLevelDefiner(_eventBus, dispencer);

        Level level = new Level(_eventBus,
                                blockField,
                                cartrigeBoxField,
                                planeSlot);

        _eventBus.Invoke(new CreatedSignal<Level>(level));

        return level;
    }
}