using System;

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
        PositionManipulator.PositionChanged += OnPositionChanged;

        PositionManipulator.RotationChanged += OnRotationChanged;
    }

    //public event Action CartrigeBoxReceived;

    public override void Destroy()
    {
        PositionManipulator.PositionChanged -= OnPositionChanged;

        PositionManipulator.RotationChanged -= OnRotationChanged;

        DeleteCartrigeBox();

        base.Destroy();
    }

    public void SetCartrigeBox(CartrigeBox cartrigeBox)
    {
        if (_cartrigeBox != null)
        {
            _cartrigeBox.Mover.TargetReached -= OnTargetPositionReached;
            _isCartrigeBoxReceived = false;
        }

        _cartrigeBox = cartrigeBox ?? throw new ArgumentNullException(nameof(cartrigeBox));

        _cartrigeBox.Mover.TargetReached += OnTargetPositionReached;
        _cartrigeBox.Mover.SetTarget(PositionManipulator.Position);
    }

    private void DeleteCartrigeBox()
    {
        if (_cartrigeBox != null)
        {
            _cartrigeBox.Mover.TargetReached -= OnTargetPositionReached;
            _cartrigeBox.Destroy();
            _cartrigeBox = null;
            _isCartrigeBoxReceived = false;
        }
    }

    private void OnPositionChanged()
    {
        if (_isCartrigeBoxReceived == false)
        {
            _cartrigeBox?.Mover.SetTarget(PositionManipulator.Position);
        }
        else
        {
            _cartrigeBox?.PositionManipulator.SetPosition(PositionManipulator.Position);
        }
    }

    private void OnRotationChanged()
    {
        if (_isCartrigeBoxReceived == false)
        {
            _cartrigeBox?.Rotator.SetTarget(PositionManipulator.Forward);
        }
        else
        {
            _cartrigeBox?.PositionManipulator.SetForward(PositionManipulator.Forward);
        }
    }

    private void OnTargetPositionReached(ITargetAction _)
    {
        _isCartrigeBoxReceived = true;
    }
}