using System;
using System.Collections.Generic;

public class Gun : Model
{
    private readonly Queue<Bullet> _bullets;

    public Gun()
    {
        _bullets = new Queue<Bullet>();
    }

    public event Action RotationFinished;
    public event Action<Bullet> ShotFired;
    public event Action<Gun> Preparing;

    public override void FinishRotate()
    {
        base.FinishRotate();
        RotationFinished?.Invoke();
    }

    public void Prepare()
    {
        Preparing?.Invoke(this);
    }

    public void Shoot(Block block)
    {
        if (_bullets.Count == 0)
        {
            throw new InvalidOperationException($"No {nameof(Bullet)}.");
        }

        Bullet bullet = _bullets.Dequeue();
        block.StayTargetForShooting();
        bullet.SetStartPosition(Position);
        bullet.SetDirectionForward(Forward);
        bullet.SetTarget(block);
        SetDirectionForward((block.Position - Position).normalized);
        ShotFired?.Invoke(bullet);
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