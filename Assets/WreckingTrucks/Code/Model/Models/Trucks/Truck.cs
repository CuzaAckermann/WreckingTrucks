using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Truck : Model
{
    private List<Model> _blocks;

    public Truck(Gun gun)
    {
        Gun = gun ?? throw new ArgumentNullException(nameof(gun));
        Gun.SetDirectionForward(Forward);
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
        _blocks = models ?? throw new ArgumentNullException(nameof(models));
    }

    public void StartShoot()
    {
        for (int i = 0; i < _blocks.Count; i++)
        {
            Gun.Shoot((Block)_blocks[i]);
        }
    }

    public override void FinishMovement()
    {
        base.FinishMovement();
        CurrentPositionReached?.Invoke(this);
    }

    public override void Destroy()
    {
        Gun.Destroy();
        base.Destroy();
    }
}