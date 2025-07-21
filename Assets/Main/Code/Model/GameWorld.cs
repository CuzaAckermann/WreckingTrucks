using System;

public class GameWorld
{
    private readonly BlockSpace _blockSpace;
    private readonly TruckSpace _truckSpace;
    private readonly CartrigeBoxSpace _cartrigeBoxSpace;
    private readonly RoadSpace _roadSpace;
    private readonly ShootingSpace _shootingSpace;
    private readonly SupplierSpace _supplierSpace;

    private readonly ModelPresenterBinder _binder;

    public GameWorld(BlockSpace blocksSpace,
                     TruckSpace trucksSpace,
                     CartrigeBoxSpace cartrigeBoxSpace,
                     RoadSpace roadSpace,
                     ShootingSpace shootingSpace,
                     SupplierSpace supplierSpace,
                     ModelPresenterBinder binder)
    {
        _blockSpace = blocksSpace ?? throw new ArgumentNullException(nameof(blocksSpace));
        _truckSpace = trucksSpace ?? throw new ArgumentNullException(nameof(trucksSpace));
        _cartrigeBoxSpace = cartrigeBoxSpace ?? throw new ArgumentNullException(nameof(cartrigeBoxSpace));
        _roadSpace = roadSpace ?? throw new ArgumentNullException(nameof(roadSpace));
        _shootingSpace = shootingSpace ?? throw new ArgumentNullException(nameof(shootingSpace));
        _supplierSpace = supplierSpace ?? throw new ArgumentNullException(nameof(supplierSpace));
        _binder = binder ?? throw new ArgumentNullException(nameof(binder));
    }

    public event Action LevelCompleted;
    public event Action LevelFailed;

    public void Clear()
    {
        _blockSpace.Clear();
        _truckSpace.Clear();
        _cartrigeBoxSpace.Clear();
        _roadSpace.Clear();
        _shootingSpace.Clear();
        _supplierSpace.Clear();

        _binder.Clear();
    }

    public void Prepare(FillingCard blockFillingCard,
                        FillingCard truckFillingCard,
                        FillingCard cartrigeBoxFillingCard)
    {
        _blockSpace.Prepare(blockFillingCard);
        _truckSpace.Prepare(truckFillingCard);
        _cartrigeBoxSpace.Prepare(cartrigeBoxFillingCard);
        
        _binder.AddNotifier(_blockSpace);
        _binder.AddNotifier(_truckSpace);
        _binder.AddNotifier(_cartrigeBoxSpace);
        _binder.AddNotifier(_shootingSpace);
    }

    public void Enable()
    {
        //_blockSpace.BlocksEnded += OnLevelCompleted;
        //_cartrigeBoxSpace.CartrigeBoxEnded += OnLevelFailed;

        _blockSpace.Enable();
        _truckSpace.Enable();
        _cartrigeBoxSpace.Enable();
        _roadSpace.Enable();
        _shootingSpace.Enable();
        _supplierSpace.Enable();

        _binder.Enable();
    }

    public void Disable()
    {
        //_blockSpace.BlocksEnded -= OnLevelCompleted;
        //_cartrigeBoxSpace.CartrigeBoxEnded -= OnLevelFailed;

        _blockSpace.Disable();
        _truckSpace.Disable();
        _cartrigeBoxSpace.Disable();
        _roadSpace.Disable();
        _shootingSpace.Disable();
        _supplierSpace.Disable();

        _binder.Disable();
    }

    public void AddTruckOnRoad(Truck truck)
    {
        if (_truckSpace.TryRemoveTruck(truck))
        {
            _roadSpace.AddTruck(truck);
            _shootingSpace.AddGun(truck.Gun);
            CartrigeBox cartrigeBox = _cartrigeBoxSpace.GetCartrigeBox();
            cartrigeBox.SetTargetPosition(truck.Position);
            _supplierSpace.AddCartrigeBox(cartrigeBox);
            truck.Prepare(_blockSpace.Field, cartrigeBox);
        }
    }

    private void OnLevelCompleted()
    {
        LevelCompleted?.Invoke();
    }

    private void OnLevelFailed()
    {
        LevelFailed?.Invoke();
    }
}