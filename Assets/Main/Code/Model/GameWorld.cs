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
    private readonly ModelFinalizer _modelFinalizer;

    public GameWorld(BlockSpace blocksSpace,
                     TruckSpace trucksSpace,
                     CartrigeBoxSpace cartrigeBoxSpace,
                     RoadSpace roadSpace,
                     ShootingSpace shootingSpace,
                     SupplierSpace supplierSpace,
                     ModelPresenterBinder binder,
                     ModelFinalizer modelFinalizer)
    {
        _blockSpace = blocksSpace ?? throw new ArgumentNullException(nameof(blocksSpace));
        _truckSpace = trucksSpace ?? throw new ArgumentNullException(nameof(trucksSpace));
        _cartrigeBoxSpace = cartrigeBoxSpace ?? throw new ArgumentNullException(nameof(cartrigeBoxSpace));
        _roadSpace = roadSpace ?? throw new ArgumentNullException(nameof(roadSpace));
        _shootingSpace = shootingSpace ?? throw new ArgumentNullException(nameof(shootingSpace));
        _supplierSpace = supplierSpace ?? throw new ArgumentNullException(nameof(supplierSpace));

        _binder = binder ?? throw new ArgumentNullException(nameof(binder));
        _modelFinalizer = modelFinalizer ?? throw new ArgumentNullException(nameof(modelFinalizer));
    }

    public event Action LevelPassed;
    public event Action LevelFailed;

    public Field BlockField => _blockSpace.Field;

    public TruckField TruckField => _truckSpace.Field;

    public void Destroy()
    {
        _modelFinalizer.Enable();

        _blockSpace.Clear();
        _truckSpace.Clear();
        _cartrigeBoxSpace.Clear();
        _roadSpace.Clear();
        _shootingSpace.Clear();
        _supplierSpace.Clear();

        _binder.Clear();
        _modelFinalizer.Disable();
        _modelFinalizer.Clear();
    }

    public void Prepare()
    {
        _blockSpace.Prepare();
        _truckSpace.Prepare();
        _cartrigeBoxSpace.Prepare();
        
        _binder.AddNotifier(_blockSpace);
        _binder.AddNotifier(_truckSpace);
        _binder.AddNotifier(_cartrigeBoxSpace);
        _binder.AddNotifier(_shootingSpace);

        _modelFinalizer.AddNotifier(_blockSpace);
        _modelFinalizer.AddNotifier(_truckSpace);
        _modelFinalizer.AddNotifier(_cartrigeBoxSpace);
        _modelFinalizer.AddNotifier(_roadSpace);
    }

    public void Enable()
    {
        SubscribeToElements();

        _blockSpace.Enable();
        _truckSpace.Enable();
        _cartrigeBoxSpace.Enable();
        _roadSpace.Enable();
        _shootingSpace.Enable();
        _supplierSpace.Enable();

        _binder.Enable();
        _modelFinalizer.Disable();

        _modelFinalizer.Enable();
    }

    public void Disable()
    {
        _blockSpace.Disable();
        _truckSpace.Disable();
        _cartrigeBoxSpace.Disable();
        _roadSpace.Disable();
        _shootingSpace.Disable();
        _supplierSpace.Disable();

        _binder.Disable();

        UnsubscribeFromElements();
    }

    public void AddTruckOnRoad(Truck truck)
    {
        if (_truckSpace.IsFirstInRow(truck))
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
    }

    private void SubscribeToElements()
    {
        _blockSpace.Wasted += OnLevelCompleted;
        _cartrigeBoxSpace.Wasted += OnLevelFailed;
    }

    private void UnsubscribeFromElements()
    {
        _blockSpace.Wasted -= OnLevelCompleted;
        _cartrigeBoxSpace.Wasted -= OnLevelFailed;
    }

    private void OnLevelCompleted()
    {
        LevelPassed?.Invoke();
    }

    private void OnLevelFailed()
    {
        LevelFailed?.Invoke();
    }
}