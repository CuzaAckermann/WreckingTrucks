using System;
using System.Collections.Generic;

public class Gun : Model
{
    private readonly Queue<Bullet> _bullets;

    public Gun(Queue<Bullet> bullets)
    {
        _bullets = bullets ?? throw new ArgumentNullException(nameof(bullets));
    }

    public event Action RotationFinished;
    public event Action<Bullet> ShotFired;

    public override void FinishRotate()
    {
        base.FinishRotate();
        RotationFinished?.Invoke();
    }

    public void Shoot(Block block)
    {
        if (_bullets.Count == 0)
        {
            throw new InvalidOperationException($"No {nameof(Bullet)}.");
        }

        Bullet bullet = _bullets.Dequeue();
        bullet.SetStartPosition(Position);
        bullet.SetDirectionForward(Forward);
        bullet.SetTarget(block);
        ShotFired?.Invoke(bullet);
    }

    //public void PutBullet(Bullet bullet)
    //{
    //    if (bullet == null)
    //    {
    //        throw new ArgumentNullException(nameof(bullet));
    //    }

    //    if (_bullets.Contains(bullet))
    //    {
    //        throw new InvalidOperationException(nameof(bullet));
    //    }

    //    _bullets.Enqueue(bullet);
    //}
}