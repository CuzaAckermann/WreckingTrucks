using System;

public class GameWorldCreator
{
    private readonly BlockFieldCreator _blockFieldCreator;
    private readonly TruckFieldCreator _truckFieldCreator;
    private readonly CartrigeBoxFieldCreator _cartrigeBoxFieldCreator;

    private readonly RecordStorageCreator _recordStorageCreator;

    private readonly FillingStrategiesCreator _fillingStrategiesCreator;

    private readonly TruckFillerCreator _truckFillerCreator;
    private readonly CartrigeBoxFillerCreator _cartrigeBoxFillerCreator;

    private readonly RoadCreator _roadCreator;
    private readonly PlaneSlotCreator _planeSlotCreator;

    private readonly DispencerCreator _dispencerCreator;

    private readonly GameWorldSettingsCreator _gameWorldSettingsCreator;
    private readonly StorageLevelSettings _storageLevelSettings;


    private int _currentIndexOfLevel;

    private FieldSize _blockFieldSize;
    private int _amountCartrigeBoxes;

    public GameWorldCreator(BlockFieldCreator blockFieldCreator, TruckFieldCreator truckFieldCreator, CartrigeBoxFieldCreator cartrigeBoxFieldCreator,
                            RecordStorageCreator recordStorageCreator,
                            FillingStrategiesCreator fillingStrategiesCreator, TruckFillerCreator truckFillerCreator, CartrigeBoxFillerCreator cartrigeBoxFillerCreator,
                            RoadCreator roadCreator,
                            PlaneSlotCreator planeSlotCreator,
                            DispencerCreator dispencerCreator,
                            GameWorldSettingsCreator gameWorldSettingsCreator,
                            StorageLevelSettings storageLevelSettings)
    {
        _blockFieldCreator = blockFieldCreator ?? throw new ArgumentNullException(nameof(blockFieldCreator));
        _truckFieldCreator = truckFieldCreator ?? throw new ArgumentNullException(nameof(truckFieldCreator));
        _cartrigeBoxFieldCreator = cartrigeBoxFieldCreator ?? throw new ArgumentNullException(nameof(cartrigeBoxFieldCreator));

        _recordStorageCreator = recordStorageCreator ?? throw new ArgumentNullException(nameof(recordStorageCreator));

        _fillingStrategiesCreator = fillingStrategiesCreator ?? throw new ArgumentNullException(nameof(fillingStrategiesCreator));

        _truckFillerCreator = truckFillerCreator ?? throw new ArgumentNullException(nameof(truckFillerCreator));
        _cartrigeBoxFillerCreator = cartrigeBoxFillerCreator ?? throw new ArgumentNullException(nameof(cartrigeBoxFillerCreator));

        _roadCreator = roadCreator ?? throw new ArgumentNullException(nameof(roadCreator));
        _planeSlotCreator = planeSlotCreator ?? throw new ArgumentNullException(nameof(planeSlotCreator));

        _dispencerCreator = dispencerCreator ?? throw new ArgumentNullException(nameof(dispencerCreator));

        _gameWorldSettingsCreator = gameWorldSettingsCreator ?? throw new ArgumentNullException(nameof(gameWorldSettingsCreator));
        _storageLevelSettings = storageLevelSettings ? storageLevelSettings : throw new ArgumentNullException(nameof(storageLevelSettings));
    }

    public event Action<GameWorld> GameWorldCreated;

    public bool CanCreateNextGameWorld()
    {
        return _storageLevelSettings.HasNextLevelSettings(_currentIndexOfLevel);
    }

    public bool CanCreatePreviousGameWorld()
    {
        return _storageLevelSettings.HasPreviousLevelSettings(_currentIndexOfLevel);
    }

    public GameWorld Recreate()
    {
        return CreateLevelGame(_currentIndexOfLevel);
    }

    public GameWorld CreateNextGameWorld()
    {
        return CreateLevelGame(_currentIndexOfLevel + 1);
    }

    public GameWorld CreatePreviousGameWorld()
    {
        return CreateLevelGame(_currentIndexOfLevel - 1);
    }

    public GameWorld CreateLevelGame(int indexOfLevel)
    {
        if (indexOfLevel < 0 || indexOfLevel >= _storageLevelSettings.AmountLevels)
        {
            throw new ArgumentOutOfRangeException(nameof(indexOfLevel));
        }

        _currentIndexOfLevel = indexOfLevel;

        LevelSettings levelSettings = _storageLevelSettings.GetLevelSettings(indexOfLevel);

        GameWorldSettings gameWorldSettings = _gameWorldSettingsCreator.PrepareGameWorldSettings(levelSettings);

        _recordStorageCreator.SetBlockFieldSettings(gameWorldSettings.LevelSettings.BlockFieldSettings);
        _blockFieldSize = gameWorldSettings.LevelSettings.BlockFieldSettings.FieldSize;
        _amountCartrigeBoxes = gameWorldSettings.LevelSettings.AmountCartrigeBoxes;

        Field blockField = _blockFieldCreator.Create(gameWorldSettings.GlobalSettings.BlockFieldTransform,
                                                     _blockFieldSize,
                                                     gameWorldSettings.GlobalSettings.BlockFieldIntervals);

        FillingStrategy<Block> fillingStrategyForBlocks = _fillingStrategiesCreator.Create<Block>(blockField,
                                                                                                  _recordStorageCreator.Create(_blockFieldSize));

        GameWorld gameWorld = CreateCommonGameWorld(gameWorldSettings, blockField, fillingStrategyForBlocks);

        return gameWorld;
    }

    public GameWorld CreateNonstopGame()
    {
        GameWorldSettings gameWorldSettings = _gameWorldSettingsCreator.GetGameWorldSettings();

        _blockFieldSize = gameWorldSettings.NonstopGameSettings.BlockFieldSize;
        _amountCartrigeBoxes = gameWorldSettings.NonstopGameSettings.AmountCartrigeBoxes;

        Field blockField = _blockFieldCreator.Create(gameWorldSettings.GlobalSettings.BlockFieldTransform,
                                                     _blockFieldSize,
                                                     gameWorldSettings.GlobalSettings.BlockFieldIntervals);

        FillingStrategy<Block> fillingStrategyForBlocks = _fillingStrategiesCreator.CreateRowFiller<Block>(blockField,
                                                                                                           _recordStorageCreator.Create(_blockFieldSize),
                                                                                                           gameWorldSettings.NonstopGameSettings.Frequency);

        GameWorld gameWorld = CreateCommonGameWorld(gameWorldSettings, blockField, fillingStrategyForBlocks);

        return gameWorld;
    }

    private GameWorld CreateCommonGameWorld(GameWorldSettings gameWorldSettings, Field blockField, FillingStrategy<Block> fillingStrategyForBlocks)
    {
        TruckField truckField = _truckFieldCreator.Create(gameWorldSettings.GlobalSettings.TruckFieldTransform,
                                                          gameWorldSettings.GlobalSettings.TruckFieldSize,
                                                          gameWorldSettings.GlobalSettings.TruckFieldIntervals);
        CartrigeBoxField cartrigeBoxField = _cartrigeBoxFieldCreator.Create(gameWorldSettings.GlobalSettings.CartrigeBoxFieldTransform,
                                                                            gameWorldSettings.GlobalSettings.CartrigeBoxFieldSize,
                                                                            gameWorldSettings.GlobalSettings.CartrigeBoxFieldIntervals);

        TruckFieldFiller truckFiller = _truckFillerCreator.Create(truckField,
                                                                  fillingStrategyForBlocks.GetUniqueStoredColors());

        Dispencer dispencer = _dispencerCreator.Create(cartrigeBoxField, _amountCartrigeBoxes);

        CartrigeBoxFieldFiller cartrigeBoxFieldFiller = _cartrigeBoxFillerCreator.Create(cartrigeBoxField,
                                                                                         dispencer);

        blockField.ContinueShiftModels();
        truckField.ContinueShiftModels();
        cartrigeBoxField.StopShiftModels();

        Road roadForTrucks = _roadCreator.Create(gameWorldSettings.RoadSpaceSettings.PathForTrucks);

        roadForTrucks.Prepare(truckField);

        PlaneSlot planeSlot = _planeSlotCreator.Create(gameWorldSettings.PlaneSpaceSettings);

        planeSlot.Prepare();

        GameWorld gameWorld = new GameWorld(blockField, truckField, cartrigeBoxField,
                                            fillingStrategyForBlocks, truckFiller, cartrigeBoxFieldFiller,
                                            roadForTrucks,
                                            _roadCreator.Create(gameWorldSettings.PlaneSpaceSettings.PathForPlane),
                                            planeSlot,
                                            dispencer);

        GameWorldCreated?.Invoke(gameWorld);

        return gameWorld;
    }
}