using System;

public class GameWorld
{
    private readonly BlockField _blockField;
    private readonly TruckField _truckField;
    private readonly CartrigeBoxField _cartrigeBoxField;

    private readonly Road _roadForTrucks;
    private readonly Road _roadForPlane;

    private readonly PlaneSlot _planeSlot;

    private readonly EndLevelWaitingState _endLevelWaitingState;

    private readonly Dispencer _cartrigeBoxDispencer;

    private readonly GameWorldFinalizer _gameWorldFinalizer;

    private readonly EventBus _eventBus;

    private bool _isSubscribedFinalizer;

    public GameWorld(BlockField blocksField,
                     TruckField truckField,
                     CartrigeBoxField cartrigeBoxField,
                     Road roadForTrucks,
                     Road roadForPlane,
                     PlaneSlot planeSlot,
                     Dispencer cartrigeBoxDispencer,
                     EventBus eventBus)
    {
        _blockField = blocksField ?? throw new ArgumentNullException(nameof(blocksField));
        _truckField = truckField ?? throw new ArgumentNullException(nameof(truckField));
        _cartrigeBoxField = cartrigeBoxField ?? throw new ArgumentNullException(nameof(cartrigeBoxField));

        _roadForTrucks = roadForTrucks ?? throw new ArgumentNullException(nameof(roadForTrucks));
        _roadForPlane = roadForPlane ?? throw new ArgumentNullException(nameof(roadForPlane));

        _planeSlot = planeSlot ?? throw new ArgumentNullException(nameof(planeSlot));

        _endLevelWaitingState = new EndLevelWaitingState(_blockField,
                                                         _cartrigeBoxField,
                                                         OnLevelCompleted,
                                                         OnLevelFailed);

        _cartrigeBoxDispencer = cartrigeBoxDispencer ?? throw new ArgumentNullException(nameof(cartrigeBoxDispencer));

        _gameWorldFinalizer = new GameWorldFinalizer(_cartrigeBoxDispencer);

        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        _isSubscribedFinalizer = false;
    }

    public event Action Destroyed;

    public event Action LevelPassed;
    public event Action LevelFailed;

    public void Destroy()
    {
        // отписка от поля с комплектами и отписка от счетчика, если выход был преждевременный
        _gameWorldFinalizer.Disable();
        UnsubscribeFromGameWorldFinalizer();

        _eventBus.Invoke(new DestroyedGameWorldSignal());

        Destroyed?.Invoke();
    }

    public void Enable()
    {
        _endLevelWaitingState.Enter();

        _eventBus.Invoke(new EnabledGameWorldSignal());
    }

    public void Disable()
    {
        _eventBus.Invoke(new DisabledGameWorldSignal());

        _endLevelWaitingState.Exit();
    }

    public void ReleaseTruck(Truck truck)
    {
        _truckField.TryGetIndexModel(truck, out int _, out int _, out int _);

        if (_truckField.IsFirstInRow(truck) == false)
        {
            return;
        }

        if (_truckField.TryRemoveTruck(truck) == false)
        {
            return;
        }

        _gameWorldFinalizer.AddTruck(truck);

        if (_cartrigeBoxDispencer.TryGetCartrigeBox(out CartrigeBox cartrigeBox) == false)
        {
            return;
        }

        truck.Prepare(cartrigeBox, _roadForTrucks);
    }

    public void ReleasePlane(Plane plane)
    {
        //if (_planeSlot.TryGetPlane(out Plane planeInSlot) == false)
        //{
        //    return;
        //}

        //if (planeInSlot != plane)
        //{
        //    return;
        //}

        //if (planeInSlot.IsWork)
        //{
        //    return;
        //}

        //if (_cartrigeBoxField.TryGetCartrigeBox(out CartrigeBox cartrigeBox))
        //{
        //    plane.Prepare(_blockField, cartrigeBox, _roadForPlane);
        //}
    }

    private void OnLevelCompleted()
    {
        LevelPassed?.Invoke();

        _gameWorldFinalizer.Disable();

        UnsubscribeFromGameWorldFinalizer();
    }

    private void OnLevelFailed()
    {
        SubscribeToGameWorldFinalizer();

        _gameWorldFinalizer.Enable();
    }

    private void SubscribeToGameWorldFinalizer()
    {
        if (_isSubscribedFinalizer == false)
        {
            _gameWorldFinalizer.LevelContinued += OnLevelContinued;
            _gameWorldFinalizer.LevelFinished += OnLevelFinished;

            _isSubscribedFinalizer = true;
        }
    }

    private void UnsubscribeFromGameWorldFinalizer()
    {
        if (_isSubscribedFinalizer)
        {
            _gameWorldFinalizer.LevelContinued -= OnLevelContinued;
            _gameWorldFinalizer.LevelFinished -= OnLevelFinished;

            _isSubscribedFinalizer = false;
        }
    }

    private void OnLevelContinued()
    {
        UnsubscribeFromGameWorldFinalizer();
    }

    private void OnLevelFinished()
    {
        LevelFailed?.Invoke();
    }
}