using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Truck : Model, ITickable
{
    private readonly Stopwatch _stopwatch;

    private List<Model> _blocks;
    private int _currentIndexOfTarget;

    public Truck(Gun gun, float shotCooldown)
    {
        if (shotCooldown <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(shotCooldown));
        }

        Gun = gun ?? throw new ArgumentNullException(nameof(gun));
        Gun.SetDirectionForward(Forward);
        _stopwatch = new Stopwatch(shotCooldown);
    }

    public event Action<Truck> CurrentPositionReached;

    public Gun Gun { get; private set; }

    public Type DestroyableType { get; private set; }

    public override void SetDirectionForward(Vector3 forward)
    {
        base.SetDirectionForward(forward);
        Gun.SetDirectionForward(forward);
    }

    public void SetDestroyableType<T>() where T : Block
    {
        DestroyableType = typeof(T);
    }

    public void SetDestroyableModels(List<Model> models)
    {
        if (models == null)
        {
            throw new ArgumentNullException(nameof(models));
        }

        //if (models.Count == 0)
        //{
        //    throw new ArgumentException($"Models is empty.");
        //}

        _blocks = models;
    }

    public void Prepare()
    {
        _currentIndexOfTarget = 0;
        Gun.Prepare();
        _stopwatch.IntervalPassed += Shoot;
        _stopwatch.Start();
    }

    public void Tick(float deltaTime)
    {
        _stopwatch.Tick(deltaTime);
    }

    public override void Move(float distance)
    {
        base.Move(distance);
        Gun.Move(distance);
    }

    public override void FinishMovement()
    {
        base.FinishMovement();
        CurrentPositionReached?.Invoke(this);
    }

    public override void Destroy()
    {
        FinishStopwatch();
        Gun.Destroy();
        base.Destroy();
    }

    private void Shoot()
    {
        if (_currentIndexOfTarget < _blocks.Count)
        {
            Gun.Shoot((Block)_blocks[_currentIndexOfTarget]);
            _currentIndexOfTarget++;
        }
        else
        {
            FinishStopwatch();
        }
    }

    private void FinishStopwatch()
    {
        _stopwatch.Stop();
        _stopwatch.IntervalPassed -= Shoot;
    }
}