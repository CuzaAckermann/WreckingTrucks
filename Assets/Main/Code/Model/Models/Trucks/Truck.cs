using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Truck : Model
{
    private readonly Vector3 _gunPosition;
    private readonly Vector3 _trunkPosition;
    private readonly BlockTracker _blockTracker;

    private readonly Stopwatch _stopwatch;
    private readonly float _shotCooldown;

    private Queue<Block> _targets;

    public Truck(Gun gun,
                 Trunk trunk,
                 BlockTracker blockTracker,
                 Stopwatch stopwatch,
                 float shotCooldown,
                 Vector3 gunPosition,
                 Vector3 trunkPosition)
    {
        if (shotCooldown <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(shotCooldown));
        }

        Gun = gun ?? throw new ArgumentNullException(nameof(gun));
        Trunk = trunk ?? throw new ArgumentNullException(nameof(trunk));
        _blockTracker = blockTracker ?? throw new ArgumentNullException(nameof(blockTracker));
        _stopwatch = stopwatch ?? throw new ArgumentNullException(nameof(stopwatch));

        SetPositionForComponents();
        Gun.SetDirectionForward(Forward);

        _gunPosition = gunPosition;
        _trunkPosition = trunkPosition;
        _shotCooldown = shotCooldown;

        _targets = new Queue<Block>(Gun.Capacity);
    }

    public event Action<Truck> CurrentPositionReached;

    public Gun Gun { get; private set; }

    public Trunk Trunk { get; private set; }

    public Type DestroyableType { get; private set; }

    public CheckPoint CurrentCheckPoint { get; private set; }

    public void SetDestroyableType<T>() where T : Block
    {
        DestroyableType = typeof(T);
    }

    public void Prepare(Field field, CartrigeBox cartrigeBox)
    {
        Gun.Prepare();
        Trunk.SetCartrigeBox(cartrigeBox);
        _blockTracker.Prepare(field, DestroyableType);
        SetPositionForComponents();
    }

    public void SetCheckPoint(CheckPoint checkPoint)
    {
        CurrentCheckPoint = checkPoint ?? throw new ArgumentNullException(nameof(checkPoint));
        SetTargetPosition(CurrentCheckPoint.Position);

        if (CurrentCheckPoint.IsStartOfShooting)
        {
            SubscribeToBlockTracker();

            Gun.ShootingEnded += FinishShooting;

            _stopwatch.SetNotificationInterval(_shotCooldown);
            _stopwatch.IntervalPassed += ActivateScan;
            _stopwatch.Start();
        }
    }

    public override void Move(float distance)
    {
        SetPositionForComponents();
        base.Move(distance);
    }

    public override void FinishMovement()
    {
        SetPositionForComponents();
        base.FinishMovement();
        CurrentPositionReached?.Invoke(this);
    }

    public override void SetDirectionForward(Vector3 forward)
    {
        Gun.SetDirectionForward(forward);
        Trunk.SetDirectionForward(forward);

        base.SetDirectionForward(forward);
    }

    public override void Rotate(float frameRotation)
    {
        Trunk.SetDirectionForward(Forward);
        base.Rotate(frameRotation);
    }

    public override void FinishRotate()
    {
        Trunk.SetDirectionForward(Forward);
        base.FinishRotate();
    }

    public override void Destroy()
    {
        FinishShooting();
        Gun.Destroy();
        Trunk.Destroy();
        base.Destroy();
    }

    private void SetPositionForComponents()
    {
        Gun.SetPosition(CalculateWorldPosition(_gunPosition));
        Trunk.SetPosition(CalculateWorldPosition(_trunkPosition));
    }

    private Vector3 CalculateWorldPosition(Vector3 localPosition)
    {
        Vector3 offset = Vector3.up * localPosition.y + Forward * localPosition.z;

        return Position + offset;
    }

    private void SubscribeToBlockTracker()
    {
        _blockTracker.TargetsDetected += OnTargetsDetected;

        _blockTracker.FieldDetected += OnFieldDetected;
        _blockTracker.FieldLeaved += FinishShooting;
    }

    private void UnsubscribeFromBlockTracker()
    {
        _blockTracker.TargetsDetected -= OnTargetsDetected;

        _blockTracker.FieldDetected -= OnFieldDetected;
        _blockTracker.FieldLeaved -= FinishShooting;
    }

    private void Shoot()
    {
        if (_targets.Count > 0)
        {
            Gun.Shoot(_targets.Dequeue());
        }
    }
    
    private void OnTargetsDetected(List<Block> targets)
    {
        int availableAddedAmount = Mathf.Min(targets.Count, Gun.AmountBullets - _targets.Count);

        for (int i = 0; i < availableAddedAmount; i++)
        {
            if (_targets.Contains(targets[i]) == false)
            {
                targets[i].StayTargetForShooting();
                _targets.Enqueue(targets[i]);
            }
        }
    }

    private void OnFieldDetected()
    {
        _stopwatch.IntervalPassed += Shoot;
    }

    private void FinishShooting()
    {
        _stopwatch.Stop();
        _stopwatch.IntervalPassed -= ActivateScan;
        _stopwatch.IntervalPassed -= Shoot;

        Gun.ShootingEnded -= FinishShooting;
        Gun.RotateToDefault(Forward);

        UnsubscribeFromBlockTracker();

        foreach (var target in _targets)
        {
            target.StayFree();
        }

        _targets.Clear();
    }

    private void ActivateScan()
    {
        _blockTracker.Scan(Position);
    }
}