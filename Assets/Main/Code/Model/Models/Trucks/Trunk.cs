using System;
using UnityEngine;

public class Trunk : Model
{
    private CartrigeBox _cartrigeBox;

    private bool _isCartrigeBoxReceived;

    public void SetCartrigeBox(CartrigeBox cartrigeBox)
    {
        if (_cartrigeBox != null)
        {
            _cartrigeBox.TargetPositionReached -= OnTargetPositionReached;
            _isCartrigeBoxReceived = false;
        }

        _cartrigeBox = cartrigeBox ?? throw new ArgumentNullException(nameof(cartrigeBox));

        _cartrigeBox.TargetPositionReached += OnTargetPositionReached;
        _cartrigeBox.SetTargetPosition(Position);
    }

    public override void SetPosition(Vector3 position)
    {
        if (_isCartrigeBoxReceived == false)
        {
            _cartrigeBox?.SetTargetPosition(position);
        }
        else
        {
            _cartrigeBox?.SetPosition(position);
        }

        base.SetPosition(position);
    }

    public override void SetDirectionForward(Vector3 forward)
    {
        _cartrigeBox?.SetDirectionForward(forward);
        base.SetDirectionForward(forward);
    }

    public override void Destroy()
    {
        if (_cartrigeBox != null)
        {
            _cartrigeBox.TargetPositionReached -= OnTargetPositionReached;
            _cartrigeBox.Destroy();
            _cartrigeBox = null;
            _isCartrigeBoxReceived = false;
        }

        base.Destroy();
    }

    private void OnTargetPositionReached(Model model)
    {
        if (model is CartrigeBox)
        {
            _isCartrigeBoxReceived = true;
        }
    }
}