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
        Gun.PositionManipulator.SetForward(PositionManipulator.Forward);
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

        Mover.SetTarget(startPoint);
        Rotator.SetTarget(startPoint);
    }

    public override void SetFirstPosition(Vector3 position)
    {
        base.SetFirstPosition(position);
        Gun.SetFirstPosition(position);
    }

    public override void SetDirectionForward(Vector3 forward)
    {
        PositionManipulator.SetForward(forward);

        Gun.PositionManipulator.SetForward(PositionManipulator.Forward);
        Trunk.PositionManipulator.SetForward(PositionManipulator.Forward);
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
}