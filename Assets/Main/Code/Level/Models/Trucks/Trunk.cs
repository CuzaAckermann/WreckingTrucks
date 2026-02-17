using System;

public class Trunk : Model
{
    private CartrigeBox _cartrigeBox;
    private bool _isCartrigeBoxReceived;

    public Trunk(Placeable positionManipulator,
                 IMover mover,
                 IRotator rotator)
          : base(positionManipulator,
                 mover,
                 rotator)
    {
        Placeable.PositionChanged += OnPositionChanged;

        Placeable.RotationChanged += OnRotationChanged;
    }

    //public event Action CartrigeBoxReceived;

    public override void Destroy()
    {
        Placeable.PositionChanged -= OnPositionChanged;

        Placeable.RotationChanged -= OnRotationChanged;

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
        _cartrigeBox.Mover.SetTarget(Placeable.Position);
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
            _cartrigeBox?.Mover.SetTarget(Placeable.Position);
        }
        else
        {
            _cartrigeBox?.Placeable.SetPosition(Placeable.Position);
        }
    }

    private void OnRotationChanged()
    {
        if (_isCartrigeBoxReceived == false)
        {
            _cartrigeBox?.Rotator.SetTarget(Placeable.Forward);
        }
        else
        {
            _cartrigeBox?.Placeable.SetForward(Placeable.Forward);
        }
    }

    private void OnTargetPositionReached(ITargetAction _)
    {
        _isCartrigeBoxReceived = true;
    }
}