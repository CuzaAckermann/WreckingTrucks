using System;

public class GameWorld
{
    private readonly Field _blockField;
    private readonly TruckField _truckField;
    private readonly CartrigeBoxField _cartrigeBoxField;

    private readonly BlockFieldFiller _blockFieldFiller;
    private readonly TruckFieldFiller _truckFieldFiller;
    private readonly CartrigeBoxFieldFiller _cartrigeBoxFieldFiller;

    private readonly Road _roadForTrucks;
    private readonly Road _roadForPlane;

    private readonly PlaneSlot _planeSlot;

    public GameWorld(Field blocksField,
                     TruckField truckField,
                     CartrigeBoxField cartrigeBoxField,
                     BlockFieldFiller blockFieldFiller,
                     TruckFieldFiller truckFieldFiller,
                     CartrigeBoxFieldFiller cartrigeBoxFieldFiller,
                     Road roadForTrucks,
                     Road roadForPlane,
                     PlaneSlot planeSlot)
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
    }

    public event Action Destroyed;

    public event Action LevelPassed;
    public event Action LevelFailed;

    public Field BlockField => _blockField;

    public TruckField TruckField => _truckField;

    public CartrigeBoxField CartrigeBoxField => _cartrigeBoxField;

    public PlaneSlot PlaneSlot => _planeSlot;

    public void Destroy()
    {
        _blockField.Clear();
        _truckField.Clear();
        _cartrigeBoxField.Clear();

        _blockFieldFiller.Clear();
        _truckFieldFiller.Clear();
        _cartrigeBoxFieldFiller.Clear();

        Destroyed?.Invoke();
    }

    public void ActivateNonstopGame()
    {
        //_blockFieldFiller.ActivateNonstopGame();
    }

    public void DeactivateNonstopGame()
    {
        //_blockFieldFiller.DeactivateNonstopGame();
    }

    public void Enable()
    {
        SubscribeToElements();

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

        UnsubscribeFromElements();
    }

    public void ReleaseTruck(Truck truck)
    {
        _truckField.TryGetIndexModel(truck, out int _, out int indexOfColumn, out int _);

        if (_truckField.IsFirstInRow(truck))
        {
            if (_truckField.TryRemoveTruck(truck))
            {
                if (_cartrigeBoxField.TryGetCartrigeBox(out CartrigeBox cartrigeBox))
                {
                    //cartrigeBox.SetTargetPosition(truck.Position);
                    truck.Prepare(cartrigeBox, _roadForTrucks);
                }
            }
        }
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

        if (_cartrigeBoxField.TryGetCartrigeBox(out CartrigeBox cartrigeBox))
        {
            //cartrigeBox.SetTargetPosition(plane.Position);
            plane.Prepare(_blockField, cartrigeBox);
        }
    }

    private void SubscribeToElements()
    {
        _blockField.Devastated += OnLevelCompleted;
        _cartrigeBoxField.Devastated += OnLevelFailed;
    }

    private void UnsubscribeFromElements()
    {
        _blockField.Devastated -= OnLevelCompleted;
        _cartrigeBoxField.Devastated -= OnLevelFailed;
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