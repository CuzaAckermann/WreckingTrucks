using System;

public class GameWorldCreator
{
    private readonly BlockSpaceCreator _blockSpaceCreator;
    private readonly TruckSpaceCreator _truckSpaceCreator;
    private readonly CartrigeBoxSpaceCreator _cartrigeBoxSpaceCreator;
    private readonly RoadSpaceCreator _roadSpaceCreator;
    private readonly ShootingSpaceCreator _shootingSpaceCreator;
    private readonly SupplierSpaceCreator _supplierSpaceCreator;

    private readonly BinderCreator _binderCreator;
    private readonly ModelFinalizerCreator _modelFinalizerCreator;

    private readonly GameWorldSettingsCreator _gameWorldSettingsCreator;
    private readonly StorageLevelSettings _storageLevelSettings;

    private int _currentIndexOfLevel;

    public GameWorldCreator(BlockSpaceCreator blockSpaceCreator,
                            TruckSpaceCreator truckSpaceCreator,
                            CartrigeBoxSpaceCreator cartrigeBoxSpaceCreator,
                            RoadSpaceCreator roadSpaceCreator,
                            ShootingSpaceCreator shootingSpaceCreator,
                            SupplierSpaceCreator supplierSpaceCreator,
                            BinderCreator binderCreator,
                            ModelFinalizerCreator modelFinalizerCreator,
                            GameWorldSettingsCreator gameWorldSettingsCreator,
                            StorageLevelSettings storageLevelSettings)
    {
        _blockSpaceCreator = blockSpaceCreator ?? throw new ArgumentNullException(nameof(blockSpaceCreator));
        _truckSpaceCreator = truckSpaceCreator ?? throw new ArgumentNullException(nameof(truckSpaceCreator));
        _cartrigeBoxSpaceCreator = cartrigeBoxSpaceCreator ?? throw new ArgumentNullException(nameof(cartrigeBoxSpaceCreator));
        _roadSpaceCreator = roadSpaceCreator ?? throw new ArgumentNullException(nameof(roadSpaceCreator));
        _shootingSpaceCreator = shootingSpaceCreator ?? throw new ArgumentNullException(nameof(shootingSpaceCreator));
        _supplierSpaceCreator = supplierSpaceCreator ?? throw new ArgumentNullException(nameof(supplierSpaceCreator));
        
        _binderCreator = binderCreator ?? throw new ArgumentNullException(nameof(binderCreator));
        _modelFinalizerCreator = modelFinalizerCreator ?? throw new ArgumentNullException(nameof(modelFinalizerCreator));

        _gameWorldSettingsCreator = gameWorldSettingsCreator ?? throw new ArgumentNullException(nameof(gameWorldSettingsCreator));
        _storageLevelSettings = storageLevelSettings ?? throw new ArgumentNullException(nameof(storageLevelSettings));
    }

    public bool CanCreateNextGameWorld()
    {
        return _storageLevelSettings.HasNextLevelSettings(_currentIndexOfLevel);
    }

    public bool CanCreatePreviousGameWorld()
    {
        return _storageLevelSettings.HasPreviousLevelSettings(_currentIndexOfLevel);
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

        GameWorld gameWorld = new GameWorld(_blockSpaceCreator.Create(gameWorldSettings.BlockSpaceSettings.FieldTransform,
                                                                      gameWorldSettings.BlockSpaceSettings),
                                            _truckSpaceCreator.Create(gameWorldSettings.TruckSpaceSettings.FieldTransform,
                                                                      gameWorldSettings.TruckSpaceSettings),
                                            _cartrigeBoxSpaceCreator.Create(gameWorldSettings.CartrigeBoxSpaceSettings.FieldTransform,
                                                                            gameWorldSettings.CartrigeBoxSpaceSettings),
                                            _roadSpaceCreator.Create(gameWorldSettings.RoadSpaceSettings.PathSettings,
                                                                     gameWorldSettings.RoadSpaceSettings),
                                            _shootingSpaceCreator.Create(gameWorldSettings.ShootingSpaceSettings),
                                            _supplierSpaceCreator.Create(gameWorldSettings.SupplierSpaceSettings),
                                            _binderCreator.Create(),
                                            _modelFinalizerCreator.Create());

        gameWorld.Prepare();

        return gameWorld;
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
}