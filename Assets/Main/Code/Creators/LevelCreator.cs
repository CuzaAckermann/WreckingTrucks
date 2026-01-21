using System;

public class LevelCreator
{
    private readonly BlockFieldCreator _blockFieldCreator;
    private readonly TruckFieldCreator _truckFieldCreator;
    private readonly CartrigeBoxFieldCreator _cartrigeBoxFieldCreator;

    private readonly BlockFillingCardCreator _blockFillingCardCreator;
    private readonly RecordStorageCreator _recordStorageCreator;

    private readonly BlockFillerCreator _blockFillerCreator;
    private readonly TruckFillerCreator _truckFillerCreator;
    private readonly CartrigeBoxFillerCreator _cartrigeBoxFillerCreator;

    private readonly RoadCreator _roadCreator;
    private readonly PlaneSlotCreator _planeSlotCreator;

    private readonly DispencerCreator _dispencerCreator;

    private readonly LevelSettingsCreator _gameWorldSettingsCreator;
    private readonly StorageLevelSettings _storageLevelSettings;

    private readonly EventBus _eventBus;

    private int _currentIndexOfLevel;

    private FieldSize _blockFieldSize;
    private int _amountCartrigeBoxes;

    public LevelCreator(BlockFieldCreator blockFieldCreator, TruckFieldCreator truckFieldCreator, CartrigeBoxFieldCreator cartrigeBoxFieldCreator,
                        BlockFillingCardCreator blockFillingCardCreator, RecordStorageCreator recordStorageCreator,
                        BlockFillerCreator blockFillerCreator, TruckFillerCreator truckFillerCreator, CartrigeBoxFillerCreator cartrigeBoxFillerCreator,
                        RoadCreator roadCreator,
                        PlaneSlotCreator planeSlotCreator,
                        DispencerCreator dispencerCreator,
                        LevelSettingsCreator gameWorldSettingsCreator,
                        StorageLevelSettings storageLevelSettings,
                        EventBus eventBus)
    {
        _blockFieldCreator = blockFieldCreator ?? throw new ArgumentNullException(nameof(blockFieldCreator));
        _truckFieldCreator = truckFieldCreator ?? throw new ArgumentNullException(nameof(truckFieldCreator));
        _cartrigeBoxFieldCreator = cartrigeBoxFieldCreator ?? throw new ArgumentNullException(nameof(cartrigeBoxFieldCreator));

        _blockFillingCardCreator = blockFillingCardCreator ?? throw new ArgumentNullException(nameof(blockFillingCardCreator));
        _recordStorageCreator = recordStorageCreator ?? throw new ArgumentNullException(nameof(recordStorageCreator));

        _blockFillerCreator = blockFillerCreator ?? throw new ArgumentException(nameof(blockFillerCreator));
        _truckFillerCreator = truckFillerCreator ?? throw new ArgumentNullException(nameof(truckFillerCreator));
        _cartrigeBoxFillerCreator = cartrigeBoxFillerCreator ?? throw new ArgumentNullException(nameof(cartrigeBoxFillerCreator));

        _roadCreator = roadCreator ?? throw new ArgumentNullException(nameof(roadCreator));
        _planeSlotCreator = planeSlotCreator ?? throw new ArgumentNullException(nameof(planeSlotCreator));

        _dispencerCreator = dispencerCreator ?? throw new ArgumentNullException(nameof(dispencerCreator));

        _gameWorldSettingsCreator = gameWorldSettingsCreator ?? throw new ArgumentNullException(nameof(gameWorldSettingsCreator));
        _storageLevelSettings = storageLevelSettings ? storageLevelSettings : throw new ArgumentNullException(nameof(storageLevelSettings));
        
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public bool CanCreateNextGameWorld()
    {
        return _storageLevelSettings.HasNextLevelSettings(_currentIndexOfLevel);
    }

    public bool CanCreatePreviousGameWorld()
    {
        return _storageLevelSettings.HasPreviousLevelSettings(_currentIndexOfLevel);
    }

    public Level Recreate()
    {
        return CreateLevelGame(_currentIndexOfLevel);
    }

    public Level CreateNextGameWorld()
    {
        return CreateLevelGame(_currentIndexOfLevel + 1);
    }

    public Level CreatePreviousGameWorld()
    {
        return CreateLevelGame(_currentIndexOfLevel - 1);
    }

    public Level CreateLevelGame(int indexOfLevel)
    {
        if (indexOfLevel < 0 || indexOfLevel >= _storageLevelSettings.AmountLevels)
        {
            throw new ArgumentOutOfRangeException(nameof(indexOfLevel));
        }

        _currentIndexOfLevel = indexOfLevel;

        LevelSettings levelSettings = _storageLevelSettings.GetLevelSettings(indexOfLevel);

        CommonLevelSettings gameWorldSettings = _gameWorldSettingsCreator.PrepareGameWorldSettings(levelSettings);

        _blockFillingCardCreator.SetBlockFieldSettings(gameWorldSettings.LevelSettings.BlockFieldSettings);
        //_recordStorageCreator.SetBlockFieldSettings(gameWorldSettings.LevelSettings.BlockFieldSettings);
        _blockFieldSize = gameWorldSettings.LevelSettings.BlockFieldSettings.FieldSize;
        _amountCartrigeBoxes = gameWorldSettings.LevelSettings.AmountCartrigeBoxes;

        BlockTracker blockTracker = new BlockTracker(_eventBus);

        BlockField blockField = _blockFieldCreator.Create(gameWorldSettings.GlobalSettings.BlockFieldTransform,
                                                          _blockFieldSize,
                                                          gameWorldSettings.GlobalSettings.BlockFieldIntervals,
                                                          _eventBus);

        BlockFieldFiller blockFieldFiller = _blockFillerCreator.Create(blockField,
                                                                       _blockFillingCardCreator.Create(_blockFieldSize),
                                                                       _eventBus);

        Level gameWorld = CreateCommonGameWorld(gameWorldSettings,
                                                    blockField,
                                                    blockFieldFiller,
                                                    blockTracker);

        return gameWorld;
    }

    public Level CreateNonstopGame()
    {
        CommonLevelSettings gameWorldSettings = _gameWorldSettingsCreator.GetGameWorldSettings();

        _blockFieldSize = gameWorldSettings.NonstopGameSettings.BlockFieldSize;
        _amountCartrigeBoxes = gameWorldSettings.NonstopGameSettings.AmountCartrigeBoxes;

        BlockTracker blockTracker = new BlockTracker(_eventBus);

        BlockField blockField = _blockFieldCreator.Create(gameWorldSettings.GlobalSettings.BlockFieldTransform,
                                                          _blockFieldSize,
                                                          gameWorldSettings.GlobalSettings.BlockFieldIntervals,
                                                          _eventBus);

        BlockFieldFiller blockFieldFiller = _blockFillerCreator.CreateNonstop(blockField,
                                                                              _recordStorageCreator.Create(_blockFieldSize),
                                                                              gameWorldSettings.NonstopGameSettings.Frequency,
                                                                              _eventBus);

        Level gameWorld = CreateCommonGameWorld(gameWorldSettings,
                                                    blockField,
                                                    blockFieldFiller,
                                                    blockTracker);

        return gameWorld;
    }

    private Level CreateCommonGameWorld(CommonLevelSettings gameWorldSettings,
                                            BlockField blockField,
                                            BlockFieldFiller blockFieldFiller,
                                            BlockTracker blockTracker)
    {
        TruckField truckField = _truckFieldCreator.Create(gameWorldSettings.GlobalSettings.TruckFieldTransform,
                                                          gameWorldSettings.GlobalSettings.TruckFieldSize,
                                                          gameWorldSettings.GlobalSettings.TruckFieldIntervals,
                                                          _eventBus);
        CartrigeBoxField cartrigeBoxField = _cartrigeBoxFieldCreator.Create(gameWorldSettings.GlobalSettings.CartrigeBoxFieldTransform,
                                                                            gameWorldSettings.GlobalSettings.CartrigeBoxFieldSize,
                                                                            gameWorldSettings.GlobalSettings.CartrigeBoxFieldIntervals,
                                                                            _eventBus);

        TruckFieldFiller truckFiller = _truckFillerCreator.Create(truckField,
                                                                  blockFieldFiller.GetUniqueStoredColors(),
                                                                  _eventBus);

        Dispencer dispencer = _dispencerCreator.Create(cartrigeBoxField, _amountCartrigeBoxes, _eventBus);

        CartrigeBoxFieldFiller cartrigeBoxFieldFiller = _cartrigeBoxFillerCreator.Create(cartrigeBoxField,
                                                                                         dispencer,
                                                                                         _eventBus);

        Road roadForTrucks = _roadCreator.Create(gameWorldSettings.RoadSpaceSettings.PathForTrucks);

        roadForTrucks.Prepare(truckField);

        PlaneSlot planeSlot = _planeSlotCreator.Create(gameWorldSettings.PlaneSpaceSettings, _eventBus);

        planeSlot.Prepare();

        TruckActivator truckActivator = new TruckActivator(_eventBus,
                                                           truckField,
                                                           dispencer,
                                                           roadForTrucks,
                                                           blockTracker);
        PlaneActivator planeActivator = new PlaneActivator(_eventBus,
                                                           blockField,
                                                           dispencer,
                                                           _roadCreator.Create(gameWorldSettings.PlaneSpaceSettings.PathForPlane),
                                                           planeSlot);
        SelectedDestroyer<Block> selectedBlockDestroyer = new SelectedDestroyer<Block>(_eventBus);

        EndLevelDefiner endLevelDefiner = new EndLevelDefiner(_eventBus, dispencer);

        Level level = new Level(_eventBus,
                                blockField,
                                cartrigeBoxField,
                                planeSlot);

        _eventBus.Invoke(new CreatedSignal<Level>(level));

        return level;
    }
}