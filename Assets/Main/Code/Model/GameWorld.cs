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

    private bool _isWaitingTruckCounter;

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

        _isWaitingTruckCounter = false;
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
        UnsubscribeFromLevelFailed();

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
        if (_activeTruckCounter.Amount == 0)
        {
            LevelFailed?.Invoke();
        }
        else
        {
            SubscribeToLevelFailed();
        }
    }

    private void OnActivedTrucksIsEmpty()
    {
        UnsubscribeFromLevelFailed();

        if (_cartrigeBoxField.CurrentAmount == 0)
        {
            LevelFailed?.Invoke();
        }
    }

    private void SubscribeToLevelFailed()
    {
        if (_isWaitingTruckCounter == false)
        {
            _cartrigeBoxField.CartrigeBoxAppeared += UnsubscribeFromLevelFailed;
            _activeTruckCounter.ActivedTrucksIsEmpty += OnActivedTrucksIsEmpty;

            _isWaitingTruckCounter = true;
        }
        else
        {
            Logger.Log("Already subscribed");
        }
    }

    private void UnsubscribeFromLevelFailed()
    {
        if (_isWaitingTruckCounter)
        {
            _cartrigeBoxField.CartrigeBoxAppeared -= UnsubscribeFromLevelFailed;
            _activeTruckCounter.ActivedTrucksIsEmpty -= OnActivedTrucksIsEmpty;

            _isWaitingTruckCounter = false;
        }
        else
        {
            Logger.Log("Already unsubscribed");
        }
    }
}