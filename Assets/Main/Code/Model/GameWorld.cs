using System;

public class GameWorld
{
    private readonly EventBus _eventBus;

    private readonly BlockField _blockField;
    private readonly TruckField _truckField;
    private readonly Dispencer _cartrigeBoxDispencer;

    private readonly Road _roadForTrucks;
    private readonly Road _roadForPlane;

    private readonly PlaneSlot _planeSlot;

    private readonly GameWorldFinalizer _gameWorldFinalizer;

    public GameWorld(BlockField blocksField, TruckField truckField, Dispencer cartrigeBoxDispencer,
                     Road roadForTrucks,
                     Road roadForPlane,
                     PlaneSlot planeSlot,
                     EventBus eventBus)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        _blockField = blocksField ?? throw new ArgumentNullException(nameof(blocksField));
        _truckField = truckField ?? throw new ArgumentNullException(nameof(truckField));
        _cartrigeBoxDispencer = cartrigeBoxDispencer ?? throw new ArgumentNullException(nameof(cartrigeBoxDispencer));

        _roadForTrucks = roadForTrucks ?? throw new ArgumentNullException(nameof(roadForTrucks));
        _roadForPlane = roadForPlane ?? throw new ArgumentNullException(nameof(roadForPlane));

        _planeSlot = planeSlot ?? throw new ArgumentNullException(nameof(planeSlot));

        _gameWorldFinalizer = new GameWorldFinalizer(_eventBus, _cartrigeBoxDispencer);
    }

    public void Destroy()
    {
        _eventBus.Unsubscribe<DevastatedBlockFieldSignal>(Complete);
        _eventBus.Unsubscribe<DevastatedCartrigeBoxFieldSignal>(WaitLastChanse);

        // отписка от поля с комплектами и отписка от счетчика, если выход был преждевременный
        _gameWorldFinalizer.Disable();
        UnsubscribeFromLevelContinuedSignal();

        _eventBus.Invoke(new DestroyedGameWorldSignal());
    }

    public void Enable()
    {
        _eventBus.Subscribe<DevastatedBlockFieldSignal>(Complete);
        _eventBus.Subscribe<DevastatedCartrigeBoxFieldSignal>(WaitLastChanse);

        _eventBus.Invoke(new EnabledGameWorldSignal());
    }

    public void Disable()
    {
        _eventBus.Unsubscribe<DevastatedBlockFieldSignal>(Complete);
        _eventBus.Unsubscribe<DevastatedCartrigeBoxFieldSignal>(WaitLastChanse);

        _eventBus.Invoke(new DisabledGameWorldSignal());
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
        if (_planeSlot.TryGetPlane(out Plane planeInSlot) == false)
        {
            return;
        }

        if (planeInSlot != plane)
        {
            return;
        }

        if (planeInSlot.IsWork)
        {
            return;
        }

        if (_cartrigeBoxDispencer.TryGetCartrigeBox(out CartrigeBox cartrigeBox))
        {
            plane.Prepare(_blockField, cartrigeBox, _roadForPlane);
        }
    }

    private void Complete(DevastatedBlockFieldSignal _)
    {
        _eventBus.Invoke(new LevelPassedSignal());

        _gameWorldFinalizer.Disable();

        UnsubscribeFromLevelContinuedSignal();
    }

    private void WaitLastChanse(DevastatedCartrigeBoxFieldSignal _)
    {
        _eventBus.Subscribe<LevelContinuedSignal>(Continue);

        _gameWorldFinalizer.Enable();
    }

    private void UnsubscribeFromLevelContinuedSignal()
    {
        _eventBus.Unsubscribe<LevelContinuedSignal>(Continue);
    }

    private void Continue(LevelContinuedSignal _)
    {
        _eventBus.Unsubscribe<LevelContinuedSignal>(Continue);
    }
}