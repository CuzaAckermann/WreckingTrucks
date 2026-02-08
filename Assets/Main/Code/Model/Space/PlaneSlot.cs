using System;
using UnityEngine;

public class PlaneSlot : Model, IAmountChangedNotifier
{
    private readonly ModelFactory<Plane> _planeFactory;

    private Plane _plane;
    private int _amountOfUses;

    public PlaneSlot(PositionManipulator positionManipulator,
                     IMover mover,
                     IRotator rotator,
                     ModelFactory<Plane> planeFactory,
                     Transform position,
                     int amountOfUses)
              : base(positionManipulator,
                     mover,
                     rotator)
    {
        if (amountOfUses <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amountOfUses));
        }

        _planeFactory = planeFactory ?? throw new ArgumentNullException(nameof(planeFactory));
        _amountOfUses = amountOfUses;

        SetPosition(position.position);
        SetDirectionForward(position.forward);
    }

    public event Action<float> AmountChanged;

    public float CurrentAmount => _amountOfUses;

    public void Prepare()
    {
        _plane = _planeFactory.Create();
        _plane.SetColor(ColorType.Gray);

        _plane.SetFirstPosition(Position + Vector3.right * 10);
        _plane.SetDirectionForward(Forward);

        _plane.SetTargetPosition(Position);

        AmountChanged?.Invoke(_amountOfUses);
    }

    public bool TryGetPlane(out Plane plane)
    {
        plane = null;

        if (_amountOfUses > 0)
        {
            plane = _plane;
            _amountOfUses--;
            AmountChanged?.Invoke(_amountOfUses);
            return true;
        }

        return false;
    }

    public int GetMaxAmount()
    {
        return _amountOfUses;
    }
}