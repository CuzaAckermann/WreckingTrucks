using System;

public class GameWorldCreator
{
    private readonly BlockFieldCreator _blockFieldCreator;
    private readonly TruckFieldCreator _truckFieldCreator;
    private readonly CartrigeBoxFieldCreator _cartrigeBoxFieldCreator;

    private readonly BlockFillerCreator _blockFillerCreator;
    private readonly TruckFillerCreator _truckFillerCreator;
    private readonly CartrigeBoxFillerCreator _cartrigeBoxFillerCreator;

    private readonly RoadCreator _roadCreator;

    private readonly GameWorldSettingsCreator _gameWorldSettingsCreator;
    private readonly StorageLevelSettings _storageLevelSettings;
    private readonly NonstopGameSettings _nonstopGameWorldSettings;

    private readonly NonstopGameBlockFieldSettingsCreator _nonstopGameBlockFieldSettingsCreator;

    private readonly PlaneSlotCreator _planeSlotCreator;

    //private readonly TruckFillingCardCreator _truckFillingCardCreator;

    private int _currentIndexOfLevel;

    public GameWorldCreator(BlockFieldCreator blockFieldCreator,
                            TruckFieldCreator truckFieldCreator,
                            CartrigeBoxFieldCreator cartrigeBoxFieldCreator,
                            BlockFillerCreator blockFillerCreator,
                            TruckFillerCreator truckFillerCreator,
                            CartrigeBoxFillerCreator cartrigeBoxFillerCreator,
                            RoadCreator roadCreator,
                            GameWorldSettingsCreator gameWorldSettingsCreator,
                            StorageLevelSettings storageLevelSettings,
                            NonstopGameSettings nonstopGameWorldSettings,
                            PlaneSlotCreator planeSlotCreator)
    {
        _blockFieldCreator = blockFieldCreator ?? throw new ArgumentNullException(nameof(blockFieldCreator));
        _truckFieldCreator = truckFieldCreator ?? throw new ArgumentNullException(nameof(truckFieldCreator));
        _cartrigeBoxFieldCreator = cartrigeBoxFieldCreator ?? throw new ArgumentNullException(nameof(cartrigeBoxFieldCreator));

        _blockFillerCreator = blockFillerCreator ?? throw new ArgumentNullException(nameof(blockFillerCreator));
        _truckFillerCreator = truckFillerCreator ?? throw new ArgumentNullException(nameof(truckFillerCreator));
        _cartrigeBoxFillerCreator = cartrigeBoxFillerCreator ?? throw new ArgumentNullException(nameof(cartrigeBoxFillerCreator));

        _roadCreator = roadCreator ?? throw new ArgumentNullException(nameof(roadCreator));

        _gameWorldSettingsCreator = gameWorldSettingsCreator ?? throw new ArgumentNullException(nameof(gameWorldSettingsCreator));
        _storageLevelSettings = storageLevelSettings ? storageLevelSettings : throw new ArgumentNullException(nameof(storageLevelSettings));

        _nonstopGameWorldSettings = nonstopGameWorldSettings ? nonstopGameWorldSettings : throw new ArgumentNullException(nameof(nonstopGameWorldSettings));

        _planeSlotCreator = planeSlotCreator ?? throw new ArgumentNullException(nameof(planeSlotCreator));

        //_truckFillingCardCreator = truckFillingCardCreator ?? throw new ArgumentNullException(nameof(truckFillingCardCreator));
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
        return Create(_currentIndexOfLevel);
    }

    public GameWorld CreateNextGameWorld()
    {
        return Create(_currentIndexOfLevel + 1);
    }

    public GameWorld CreatePreviousGameWorld()
    {
        return Create(_currentIndexOfLevel - 1);
    }

    public GameWorld Create(int indexOfLevel)
    {
        if (indexOfLevel < 0 || indexOfLevel >= _storageLevelSettings.AmountLevels)
        {
            throw new ArgumentOutOfRangeException(nameof(indexOfLevel));
        }

        _currentIndexOfLevel = indexOfLevel;

        LevelSettings levelSettings = _storageLevelSettings.GetLevelSettings(indexOfLevel);

        GameWorldSettings gameWorldSettings = _gameWorldSettingsCreator.PrepareGameWorldSettings(levelSettings);

        GameWorld gameWorld = CreateCommonGameWorld(gameWorldSettings);

        return gameWorld;
    }

    public GameWorld CreateNonstopGame()
    {
        GameWorldSettings gameWorldSettings = _gameWorldSettingsCreator.PrepareNonstopGameWorldSettings(_nonstopGameWorldSettings);

        GameWorld gameWorld = CreateCommonGameWorld(gameWorldSettings);

        gameWorld.ActivateNonstopGame();

        return gameWorld;
    }

    private GameWorld CreateCommonGameWorld(GameWorldSettings gameWorldSettings)
    {

        Field blockField = _blockFieldCreator.Create(gameWorldSettings.BlockSpaceSettings);
        TruckField truckField = _truckFieldCreator.Create(gameWorldSettings.TruckSpaceSettings);
        CartrigeBoxField cartrigeBoxField = _cartrigeBoxFieldCreator.Create(gameWorldSettings.CartrigeBoxSpaceSettings);

        _blockFillerCreator.SetBlockLayerSettings(gameWorldSettings.BlockSpaceSettings.FieldSettings.Layers);
        BlockFieldFiller blockFieldFiller = _blockFillerCreator.Create(blockField, gameWorldSettings.BlockSpaceSettings);

        _truckFillerCreator.Prepare(blockFieldFiller.GetColorTypes());

        _cartrigeBoxFillerCreator.SetFieldSettings(gameWorldSettings.CartrigeBoxSpaceSettings.FieldSettings);

        TruckFieldFiller truckFiller = _truckFillerCreator.Create(truckField,
                                                                  gameWorldSettings.TruckSpaceSettings,
                                                                  blockFieldFiller.GetColorTypes());

        CartrigeBoxFieldFiller cartrigeBoxFieldFiller = _cartrigeBoxFillerCreator.Create(cartrigeBoxField,
                                                                                         gameWorldSettings.CartrigeBoxSpaceSettings);

        blockField.ContinueShiftModels();
        truckField.ContinueShiftModels();
        cartrigeBoxField.StopShiftModels();

        Road roadForTrucks = _roadCreator.Create(gameWorldSettings.RoadSpaceSettings.PathForTrucks);

        roadForTrucks.Prepare(truckField);

        PlaneSlot planeSlot = _planeSlotCreator.Create(gameWorldSettings.PlaneSpaceSettings);

        planeSlot.Prepare();

        GameWorld gameWorld = new GameWorld(blockField,
                                            truckField,
                                            cartrigeBoxField,
                                            blockFieldFiller,
                                            truckFiller,
                                            cartrigeBoxFieldFiller,
                                            roadForTrucks,
                                            _roadCreator.Create(gameWorldSettings.PlaneSpaceSettings.PathForPlane),
                                            planeSlot);

        GameWorldCreated?.Invoke(gameWorld);

        return gameWorld;
    }
}