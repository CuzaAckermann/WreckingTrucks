using System;

public class GameWorldCreator
{
    private readonly BlockSpaceCreator _blockSpaceCreator;
    private readonly TruckSpaceCreator _truckSpaceCreator;
    private readonly CartrigeBoxSpaceCreator _cartrigeBoxSpaceCreator;
    private readonly RoadSpaceCreator _roadSpaceCreator;
    private readonly ShootingSpaceCreator _shootingSpaceCreator;
    private readonly SupplierSpaceCreator _supplierSpaceCreator;

    private readonly ModelPresenterBinder _binder;

    public GameWorldCreator(BlockSpaceCreator blockSpaceCreator,
                            TruckSpaceCreator truckSpaceCreator,
                            CartrigeBoxSpaceCreator cartrigeBoxSpaceCreator,
                            RoadSpaceCreator roadSpaceCreator,
                            ShootingSpaceCreator shootingSpaceCreator,
                            SupplierSpaceCreator supplierSpaceCreator,
                            ModelPresenterBinder binder)
    {
        _blockSpaceCreator = blockSpaceCreator ?? throw new ArgumentNullException(nameof(blockSpaceCreator));
        _truckSpaceCreator = truckSpaceCreator ?? throw new ArgumentNullException(nameof(truckSpaceCreator));
        _cartrigeBoxSpaceCreator = cartrigeBoxSpaceCreator ?? throw new ArgumentNullException(nameof(cartrigeBoxSpaceCreator));
        _roadSpaceCreator = roadSpaceCreator ?? throw new ArgumentNullException(nameof(roadSpaceCreator));
        _shootingSpaceCreator = shootingSpaceCreator ?? throw new ArgumentNullException(nameof(shootingSpaceCreator));
        _supplierSpaceCreator = supplierSpaceCreator ?? throw new ArgumentNullException(nameof(supplierSpaceCreator));
        _binder = binder ?? throw new ArgumentNullException(nameof(binder));
    }

    public GameWorld Create(PlacementSettings placementSettings, PathSettings pathSettings, GameWorldSettings gameWorldSettings)
    {
        return new GameWorld(_blockSpaceCreator.Create(placementSettings.BlockField, gameWorldSettings.BlockSpaceSettings),
                             _truckSpaceCreator.Create(placementSettings.TruckField, gameWorldSettings.TruckSpaceSettings),
                             _cartrigeBoxSpaceCreator.Create(placementSettings.CartrigeBoxField, gameWorldSettings.CartrigeBoxSpaceSettings),
                             _roadSpaceCreator.Create(pathSettings, gameWorldSettings.RoadSpaceSettings),
                             _shootingSpaceCreator.Create(gameWorldSettings.ShootingSpaceSettings),
                             _supplierSpaceCreator.Create(gameWorldSettings.SupplierSpaceSettings),
                             _binder);
    }
}