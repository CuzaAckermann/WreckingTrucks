using System;
using UnityEngine;

public class Truck : Model
{
    private readonly BlockTracker _blockTracker;
    
    private Road _road;
    private int _currentPoint;

    public Truck(float movespeed,
                 float rotatespeed,
                 Trunk trunk,
                 BlockTracker blockTracker)
          : base(movespeed, rotatespeed)
    {
        Trunk = trunk ?? throw new ArgumentNullException(nameof(trunk));
        _blockTracker = blockTracker ?? throw new ArgumentNullException(nameof(blockTracker));
    }

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

    public void Prepare(CartrigeBox cartrigeBox, Road road)
    {
        Gun.Upload();
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

    protected override void FinishMovement()
    {
        if (_road != null)
        {
            if (_road.TryGetNextPoint(_currentPoint, out Vector3 nextPoint))
            {
                _currentPoint++;
                TargetPosition = nextPoint;
                SetTargetPosition(TargetPosition);
                SetTargetRotation(TargetPosition);
            }
        }
        else
        {
            base.FinishMovement();
        }
    }

    public void Shoot()
    {
        if (Gun.CanShoot())
        {
            if (_blockTracker.TryGetTarget(DestroyableColor, out Block target))
            {
                Gun.ReadyToFire += OnReadyToFire;
                Gun.Aim(target);
            }
            else
            {
                _blockTracker.TargetDetected += OnTargetDetected;
            }
        }
        else
        {
            StopShooting();
        }
    }

    public void StopShooting()
    {
        _blockTracker.TargetDetected -= OnTargetDetected;

        Gun.StopShooting();
        Gun.ReadyToFire -= OnReadyToFire;
    }

    private void OnTargetDetected()
    {
        _blockTracker.TargetDetected -= OnTargetDetected;

        Shoot();
    }

    private void OnReadyToFire()
    {
        Gun.ReadyToFire -= OnReadyToFire;

        Shoot();
    }
}