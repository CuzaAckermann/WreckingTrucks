using System;

public class GameWorld
{
    private readonly Field _blockField;
    private readonly TruckField _truckField;
    private readonly CartrigeBoxField _cartrigeBoxField;

    private readonly FillingStrategy<Block> _blockFieldFiller;
    private readonly TruckFieldFiller _truckFieldFiller;
    private readonly CartrigeBoxFieldFiller _cartrigeBoxFieldFiller;

    private readonly Road _roadForTrucks;
    private readonly Road _roadForPlane;

    private readonly PlaneSlot _planeSlot;

    private readonly EndLevelWaitingState _endLevelWaitingState;

    private readonly Dispencer _cartrigeBoxDispencer;

    private readonly ActiveTruckCounter _activeTruckCounter;
    private readonly ActiveBulletCounter _activeBulletCounter;

    private bool _isWaitingDispencer;
    private bool _isWaitingTruckCounter;
    private bool _isWaitingBulletCounter;

    public GameWorld(Field blocksField,
                     TruckField truckField,
                     CartrigeBoxField cartrigeBoxField,
                     FillingStrategy<Block> blockFieldFiller,
                     TruckFieldFiller truckFieldFiller,
                     CartrigeBoxFieldFiller cartrigeBoxFieldFiller,
                     Road roadForTrucks,
                     Road roadForPlane,
                     PlaneSlot planeSlot,
                     Dispencer cartrigeBoxDispencer)
    {
        _blockField = blocksField ?? throw new ArgumentNullException(nameof(blocksField));
        _truckField = truckField ?? throw new ArgumentNullException(nameof(truckField));
        _cartrigeBoxField = cartrigeBoxField ?? throw new ArgumentNullException(nameof(cartrigeBoxField));

        _blockFieldFiller = blockFieldFiller ?? throw new ArgumentNullException(nameof(blockFieldFiller));
        _truckFieldFiller = truckFieldFiller ?? throw new ArgumentNullException(nameof(truckFieldFiller));
        _cartrigeBoxFieldFiller = cartrigeBoxFieldFiller ?? throw new ArgumentNullException(nameof(cartrigeBoxFieldFiller));

        _roadForTrucks = roadForTrucks ?? throw new ArgumentNullException(nameof(roadForTrucks));
        _roadForPlane = roadForPlane ?? throw new ArgumentNullException(nameof(roadForPlane));

        _planeSlot = planeSlot ?? throw new ArgumentNullException(nameof(planeSlot));

        _endLevelWaitingState = new EndLevelWaitingState(_blockField,
                                                         _cartrigeBoxField,
                                                         OnLevelCompleted,
                                                         OnLevelFailed);

        _cartrigeBoxDispencer = cartrigeBoxDispencer ?? throw new ArgumentNullException(nameof(cartrigeBoxDispencer));

        _activeTruckCounter = new ActiveTruckCounter();
        _activeBulletCounter = new ActiveBulletCounter();

        _isWaitingDispencer = false;
        _isWaitingTruckCounter = false;
        _isWaitingBulletCounter = false;
    }

    public event Action Destroyed;

    public event Action LevelPassed;
    public event Action LevelFailed;

    public Field BlockField => _blockField;

    public TruckField TruckField => _truckField;

    public CartrigeBoxField CartrigeBoxField => _cartrigeBoxField;

    public PlaneSlot PlaneSlot => _planeSlot;

    public Dispencer CartrigeBoxDispencer => _cartrigeBoxDispencer;

    public void Destroy()
    {
        // отписка от поля с комплектами и отписка от счетчика, если выход был преждевременный
        OnRecordAppeared();

        _blockField.Clear();
        _truckField.Clear();
        _cartrigeBoxField.Clear();

        _blockFieldFiller.Clear();
        _truckFieldFiller.Clear();
        _cartrigeBoxFieldFiller.Clear();

        Destroyed?.Invoke();
    }

    public void Enable()
    {
        _endLevelWaitingState.Enter();

        _blockField.Enable();
        _truckField.Enable();
        _cartrigeBoxField.Enable();

        _blockFieldFiller.Enable();
        _truckFieldFiller.Enable();
        _cartrigeBoxFieldFiller.Enable();
    }

    public void Disable()
    {
        _blockField.Disable();
        _truckField.Disable();
        _cartrigeBoxField.Disable();

        _blockFieldFiller.Disable();
        _truckFieldFiller.Disable();
        _cartrigeBoxFieldFiller.Disable();

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

        _activeTruckCounter.AddActivedTruck(truck);
        _activeBulletCounter.SubscribeToGun(truck.Gun);

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
    }

    private void OnLevelFailed()
    {
        if (_activeTruckCounter.Amount == 0 && _activeBulletCounter.Amount == 0)
        {
            LevelFailed?.Invoke();
        }

        if (_activeTruckCounter.Amount > 0)
        {
            SubscribeToTruckCounter();
        }
        else if (_activeBulletCounter.Amount > 0)
        {
            SubscribeToBulletCounter();
        }

        SubscribeToDispencer();
    }

    private void SubscribeToTruckCounter()
    {
        if (_isWaitingTruckCounter == false)
        {
            _activeTruckCounter.ActivedTrucksIsEmpty += OnActivedTrucksIsEmpty;

            _isWaitingTruckCounter = true;
        }
        else
        {
            Logger.Log("Already subscribed");
        }
    }

    private void UnsubscribeFromTruckCounter()
    {
        if (_isWaitingTruckCounter)
        {
            _activeTruckCounter.ActivedTrucksIsEmpty -= OnActivedTrucksIsEmpty;

            _isWaitingTruckCounter = false;
        }
        else
        {
            Logger.Log("Already unsubscribed");
        }
    }

    private void OnActivedTrucksIsEmpty()
    {
        UnsubscribeFromTruckCounter();

        if (_activeBulletCounter.Amount > 0)
        {
            SubscribeToBulletCounter();
        }
        else if (_cartrigeBoxDispencer.Amount == 0)
        {
            UnsubscribeFromDispencer();

            LevelFailed?.Invoke();
        }
    }

    private void SubscribeToBulletCounter()
    {
        if (_isWaitingBulletCounter == false)
        {
            _activeBulletCounter.ActivedBulletIsEmpty += OnActivedBulletIsEmpty;

            _isWaitingBulletCounter = true;
        }
        else
        {
            Logger.Log("Already subscribed");
        }
    }

    private void UnsubscribeFromBulletCounter()
    {
        if (_isWaitingBulletCounter)
        {
            _activeBulletCounter.ActivedBulletIsEmpty -= OnActivedBulletIsEmpty;

            _isWaitingBulletCounter = false;
        }
        else
        {
            Logger.Log("Already unsubscribed");
        }
    }

    private void OnActivedBulletIsEmpty()
    {
        UnsubscribeFromBulletCounter();

        if (_cartrigeBoxDispencer.Amount == 0)
        {
            UnsubscribeFromDispencer();

            LevelFailed?.Invoke();
        }
    }

    private void SubscribeToDispencer()
    {
        if (_isWaitingDispencer == false)
        {
            _cartrigeBoxDispencer.RecordAppeared += OnRecordAppeared;

            _isWaitingDispencer = true;
        }
        else
        {
            Logger.Log("Already subscribed");
        }
    }

    private void UnsubscribeFromDispencer()
    {
        if (_isWaitingDispencer)
        {
            _cartrigeBoxDispencer.RecordAppeared -= OnRecordAppeared;

            _isWaitingDispencer = false;
        }
        else
        {
            Logger.Log("Already unsubscribed");
        }
    }

    private void OnRecordAppeared()
    {
        UnsubscribeFromDispencer();

        UnsubscribeFromTruckCounter();
        UnsubscribeFromBulletCounter();
    }
}