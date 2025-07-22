using System;

public class GameWorldCreator
{
    private readonly BlockSpaceCreator _blockSpaceCreator;
    private readonly TruckSpaceCreator _truckSpaceCreator;
    private readonly CartrigeBoxSpaceCreator _cartrigeBoxSpaceCreator;
    private readonly RoadSpaceCreator _roadSpaceCreator;
    private readonly ShootingSpaceCreator _shootingSpaceCreator;
    private readonly SupplierSpaceCreator _supplierSpaceCreator;

    private readonly BlockFillingCardCreator _blockFillingCardCreator;
    private readonly TruckFillingCardCreator _truckFillingCardCreator;
    private readonly CartrigeBoxFillingCardCreator _cartrigeBoxFillingCardCreator;

    private readonly BinderCreator _binderCreator;
    private readonly ModelFinalizerCreator _modelFinalizerCreator;

    private GameWorldSettings _currentGameWorldSettings;

    public GameWorldCreator(BlockSpaceCreator blockSpaceCreator,
                            TruckSpaceCreator truckSpaceCreator,
                            CartrigeBoxSpaceCreator cartrigeBoxSpaceCreator,
                            RoadSpaceCreator roadSpaceCreator,
                            ShootingSpaceCreator shootingSpaceCreator,
                            SupplierSpaceCreator supplierSpaceCreator,
                            BlockFillingCardCreator blockFillingCardCreator,
                            TruckFillingCardCreator truckFillingCardCreator,
                            CartrigeBoxFillingCardCreator cartrigeBoxFillingCardCreator,
                            BinderCreator binderCreator,
                            ModelFinalizerCreator modelFinalizerCreator)
    {
        _blockSpaceCreator = blockSpaceCreator ?? throw new ArgumentNullException(nameof(blockSpaceCreator));
        _truckSpaceCreator = truckSpaceCreator ?? throw new ArgumentNullException(nameof(truckSpaceCreator));
        _cartrigeBoxSpaceCreator = cartrigeBoxSpaceCreator ?? throw new ArgumentNullException(nameof(cartrigeBoxSpaceCreator));
        _roadSpaceCreator = roadSpaceCreator ?? throw new ArgumentNullException(nameof(roadSpaceCreator));
        _shootingSpaceCreator = shootingSpaceCreator ?? throw new ArgumentNullException(nameof(shootingSpaceCreator));
        _supplierSpaceCreator = supplierSpaceCreator ?? throw new ArgumentNullException(nameof(supplierSpaceCreator));
        
        _blockFillingCardCreator = blockFillingCardCreator ?? throw new ArgumentNullException(nameof(blockFillingCardCreator));
        _truckFillingCardCreator = truckFillingCardCreator ?? throw new ArgumentNullException(nameof(truckFillingCardCreator));
        _cartrigeBoxFillingCardCreator = cartrigeBoxFillingCardCreator ?? throw new ArgumentNullException(nameof(cartrigeBoxFillingCardCreator));
        
        _binderCreator = binderCreator ?? throw new ArgumentNullException(nameof(binderCreator));
        _modelFinalizerCreator = modelFinalizerCreator ?? throw new ArgumentNullException(nameof(modelFinalizerCreator));
    }

    public GameWorld Create(GameWorldSettings gameWorldSettings)
    {
        _currentGameWorldSettings = gameWorldSettings;

        

        GameWorld gameWorld = new GameWorld(_blockSpaceCreator.Create(gameWorldSettings.BlockSpaceSettings.FieldSettings.Transform, gameWorldSettings.BlockSpaceSettings),
                                            _truckSpaceCreator.Create(gameWorldSettings.TruckSpaceSettings.FieldSettings.Transform, gameWorldSettings.TruckSpaceSettings),
                                            _cartrigeBoxSpaceCreator.Create(gameWorldSettings.CartrigeBoxSpaceSettings.FieldSettings.Transform, gameWorldSettings.CartrigeBoxSpaceSettings),
                                            _roadSpaceCreator.Create(gameWorldSettings.RoadSpaceSettings.PathSettings, gameWorldSettings.RoadSpaceSettings),
                                            _shootingSpaceCreator.Create(gameWorldSettings.ShootingSpaceSettings),
                                            _supplierSpaceCreator.Create(gameWorldSettings.SupplierSpaceSettings),
                                            _binderCreator.Create(),
                                            _modelFinalizerCreator.Create());

        gameWorld.Prepare();

        return gameWorld;
    }

    public GameWorld Recreate()
    {
        return Create(_currentGameWorldSettings);
    }
}