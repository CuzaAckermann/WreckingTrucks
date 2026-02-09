using System;
using UnityEngine;

public class Trunk : Model
{
    private CartrigeBox _cartrigeBox;
    private bool _isCartrigeBoxReceived;

    public Trunk(PositionManipulator positionManipulator,
                 IMover mover,
                 IRotator rotator)
          : base(positionManipulator,
                 mover,
                 rotator)
    {

    }

    //public event Action CartrigeBoxReceived;

    public void SetCartrigeBox(CartrigeBox cartrigeBox)
    {
        if (_cartrigeBox != null)
        {
            _cartrigeBox.Mover.TargetReached -= OnTargetReached;
            _isCartrigeBoxReceived = false;
        }

        _cartrigeBox = cartrigeBox ?? throw new ArgumentNullException(nameof(cartrigeBox));

        _cartrigeBox.Mover.TargetReached += OnTargetReached;
        _cartrigeBox.Mover.SetTarget(PositionManipulator.Position);
    }

    public override void SetPosition(Vector3 position)
    {
        if (_isCartrigeBoxReceived == false)
        {
            _cartrigeBox?.Mover.SetTarget(position);
        }
        else
        {
            _cartrigeBox?.PositionManipulator.SetPosition(position);
        }

        PositionManipulator.SetPosition(position);
    }

    public override void SetDirectionForward(Vector3 forward)
    {
        if (_isCartrigeBoxReceived == false)
        {
            _cartrigeBox?.Rotator.SetTarget(forward);
        }
        else
        {
            _cartrigeBox?.PositionManipulator.SetForward(forward);
        }

        PositionManipulator.SetForward(forward);
    }

    public override void Destroy()
    {
        DeleteCartrigeBox();

        base.Destroy();
    }

    public void DeleteCartrigeBox()
    {
        if (_cartrigeBox != null)
        {
            _cartrigeBox.Mover.TargetReached -= OnTargetReached;
            _cartrigeBox.Destroy();
            _cartrigeBox = null;
            _isCartrigeBoxReceived = false;
        }
    }

    private void OnTargetReached(ITargetAction _)
    {
        _isCartrigeBoxReceived = true;
    }
}