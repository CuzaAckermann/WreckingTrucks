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

        Capacity = capacity;
        _bullets = new Queue<Bullet>(capacity);
    }

    public event Action<Gun> Uploading;
    public event Action<Bullet> ShotFired;
    public event Action<Gun> ShootingEnded;

    public int Capacity { get; private set; }

    public void Clear()
    {
        _bullets.Clear();
        //ShootingEnded?.Invoke(this);
    }

    public void Upload()
    {
        Uploading?.Invoke(this);
    }

    public void Upload(int amountBullets)
    {
        if (amountBullets <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amountBullets));
        }

        Capacity = amountBullets;
        Upload();
    }

    public void Shoot(Block block)
    {
        if (_bullets.Count > 0)
        {
            Bullet bullet = _bullets.Dequeue();
            bullet.SetPosition(Position);
            bullet.SetDirectionForward(Forward);
            bullet.SetTarget(block);
            ShotFired?.Invoke(bullet);
        }

        if (_bullets.Count == 0)
        {
            ShootingEnded?.Invoke(this);
        }
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

    protected override Vector3 GetAxisOfRotation()
    {
        return Vector3.up;
    }

    protected override Vector3 GetTargetRotation(Vector3 target)
    {
        Vector3 targetDirection = target - Position;
        targetDirection.y = 0;

        return targetDirection;
    }
}