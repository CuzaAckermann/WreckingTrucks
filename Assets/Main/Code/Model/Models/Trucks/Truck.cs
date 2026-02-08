using System;
using UnityEngine;

public class Truck : Model
{
    private readonly ColorShootingState _colorShootingState;

    private BlockTracker _blockTracker;
    private Road _road;
    private int _currentPoint;

    public Truck(PositionManipulator positionManipulator,
                 IMover mover,
                 IRotator rotator,
                 Trunk trunk)
          : base(positionManipulator,
                 mover,
                 rotator)
    {
        Trunk = trunk ?? throw new ArgumentNullException(nameof(trunk));

        _colorShootingState = new ColorShootingState();
    }

    public event Action<Truck> ShootingFinished;

    public Gun Gun { get; private set; }

    public Trunk Trunk { get; private set; }

    public ColorType DestroyableColor { get; private set; }

    public void SetGun(Gun gun)
    {
        Gun = gun ?? throw new ArgumentNullException(nameof(gun));
        Gun.SetDirectionForward(Forward);
    }

    public override void SetColor(ColorType color)
    {
        base.SetColor(color);

        DestroyableColor = color;
    }

    public void Prepare(BlockTracker blockTracker, CartrigeBox cartrigeBox, Road road)
    {
        _blockTracker = blockTracker ?? throw new ArgumentNullException(nameof(blockTracker));

        Trunk.SetCartrigeBox(cartrigeBox);

        _road = road ?? throw new ArgumentNullException(nameof(road));

        Vector3 startPoint = _road.GetFirstPoint();
        _currentPoint = 0;

        SetTargetPosition(startPoint);
        SetTargetRotation(startPoint);
    }

    public override void SetFirstPosition(Vector3 position)
    {
        base.SetFirstPosition(position);
        Gun.SetFirstPosition(position);
    }

    public override void SetDirectionForward(Vector3 forward)
    {
        base.SetDirectionForward(forward);

        Gun.SetDirectionForward(Forward);
        Trunk.SetDirectionForward(Forward);
    }

    public override void Destroy()
    {
        _road = null;

        StopShooting();
        Gun.Destroy();
        Trunk.Destroy();
        base.Destroy();
    }

    public void StartShooting()
    {
        _colorShootingState.Enter(_blockTracker,
                                  Gun,
                                  DestroyableColor);
    }

    public void StopShooting()
    {
        _colorShootingState.Exit();

        ShootingFinished?.Invoke(this);
    }

    protected override void FinishMovement()
    {
        if (_road != null)
        {
            if (_road.TryGetNextPoint(_currentPoint, out Vector3 nextPoint))
            {
                _currentPoint++;

                //TargetPosition = nextPoint;
                SetTargetPosition(TargetPosition);
                SetTargetRotation(TargetPosition);
            }
        }
        else
        {
            base.FinishMovement();
        }
    }
}