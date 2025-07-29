using System;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Model
{
    private readonly Queue<Bullet> _bullets;

    public Gun(int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity));
        }

        _bullets = new Queue<Bullet>();
        Capacity = capacity;
    }

    public event Action RotationFinished;
    public event Action<Bullet> ShotFired;
    public event Action<Gun> Preparing;
    public event Action ShootingEnded;

    public int Capacity { get; private set; }

    public int AmountBullets => _bullets.Count;

    public override void FinishRotate()
    {
        base.FinishRotate();
        RotationFinished?.Invoke();
    }

    public void Clear()
    {
        _bullets.Clear();
    }

    public void Prepare()
    {
        Preparing?.Invoke(this);
    }

    public void Shoot(Block block)
    {
        if (_bullets.Count > 0)
        {
            Bullet bullet = _bullets.Dequeue();
            block.StayTargetForShooting();
            bullet.SetPosition(Position + Forward * 0.75f);
            bullet.SetDirectionForward(Forward);
            bullet.SetTarget(block);
            SetDirectionForward((block.Position - Position).normalized);
            ShotFired?.Invoke(bullet);
        }

        if (_bullets.Count == 0)
        {
            ShootingEnded?.Invoke();
        }
    }

    public void RotateToDefault(Vector3 defaultForward)
    {
        SetDirectionForward(defaultForward);
    }

    public void PutBullet(Bullet bullet)
    {
        if (bullet == null)
        {
            throw new ArgumentNullException(nameof(bullet));
        }

        if (_bullets.Contains(bullet))
        {
            throw new InvalidOperationException(nameof(bullet));
        }

        _bullets.Enqueue(bullet);
    }
}