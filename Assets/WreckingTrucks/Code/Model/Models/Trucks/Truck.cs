using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Truck : Model, ITickable
{
    private readonly Stopwatch _stopwatch;
    private readonly Vector3 _localPositionGun;
    private readonly BlockTracker _blockTracker;

    private List<Model> _blocks;

    private Field _field;

    public Truck(Gun gun, float shotCooldown, Vector3 localPositionGun)
    {
        if (shotCooldown <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(shotCooldown));
        }

        Gun = gun ?? throw new ArgumentNullException(nameof(gun));
        Gun.SetDirectionForward(Forward);
        _stopwatch = new Stopwatch(shotCooldown);
        _localPositionGun = localPositionGun;
        _blockTracker = new BlockTracker();
    }

    public event Action<Truck> CurrentPositionReached;
    public event Action AmountBulletChanged;

    public Gun Gun { get; private set; }

    public Type DestroyableType { get; private set; }

    public CheckPoint CurrentCheckPoint { get; private set; }

    public void SetDestroyableType<T>() where T : Block
    {
        DestroyableType = typeof(T);
    }

    public void SetDestroyableModels(List<Model> models)
    {
        _blocks = models ?? throw new ArgumentNullException(nameof(models));
    }

    public void Prepare(Field field)
    {
        Gun.Prepare();
        _field = field ?? throw new ArgumentNullException(nameof(field));
        _blockTracker.SetField(field);
        _blockTracker.SetAcceptableAngle(20);
        _blockTracker.AcceptableAngleReached += Shoot;
        _blockTracker.FieldEscaped += FinishStopwatch;
    }
    
    public void SetCheckPoint(CheckPoint checkPoint)
    {
        CurrentCheckPoint = checkPoint ?? throw new ArgumentNullException(nameof(checkPoint));
        SetTargetPosition(CurrentCheckPoint.Position);

        if (CurrentCheckPoint.IsStartOfShooting)
        {
            _stopwatch.IntervalPassed += TickTracker;
            _stopwatch.Start();
        }
    }
    
    public void Tick(float deltaTime)
    {
        _stopwatch.Tick(deltaTime);
    }

    public override void Move(float distance)
    {
        base.Move(distance);
        Gun.SetStartPosition(Position + _localPositionGun);
        Gun.Move(distance);
    }

    public override void FinishMovement()
    {
        base.FinishMovement();
        CurrentPositionReached?.Invoke(this);
    }

    public override void SetDirectionForward(Vector3 forward)
    {
        base.SetDirectionForward(forward);
        Gun.SetDirectionForward(forward);
    }

    public override void Destroy()
    {
        FinishStopwatch();
        Gun.Destroy();
        base.Destroy();
    }

    private void Shoot(Block block)
    {
        if (block.GetType() == DestroyableType)
        {
            Gun.Shoot(block);
            AmountBulletChanged?.Invoke();
        }
    }

    private void TickTracker()
    {
        _blockTracker.Tick(Position);
    }

    private void FinishStopwatch()
    {
        _stopwatch.Stop();
        _stopwatch.IntervalPassed -= TickTracker;

        _blockTracker.AcceptableAngleReached -= Shoot;
        _blockTracker.FieldEscaped -= FinishStopwatch;
    }
}