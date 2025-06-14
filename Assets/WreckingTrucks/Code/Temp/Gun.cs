using System;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Model, IPositionsModelsChangedNotifier
{
    private BulletFactory _bulletFactory;
    private Queue<Bullet> _bullets;
    private Vector3 _pointShot;

    public Gun(BulletFactory bulletFactory, int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity));
        }

        _bulletFactory = bulletFactory ?? throw new ArgumentNullException(nameof(bulletFactory));
        _bullets = new Queue<Bullet>(capacity);
        ChargeMagazine(capacity);
    }

    public event Action<Model> ModelAdded;
    public event Action<List<Model>> TargetPositionsModelsChanged;

    public void SetPointShot(Vector3 pointShot)
    {
        _pointShot = pointShot;
    }

    public void Shoot(Block block)
    {
        if (_bullets.Count == 0)
        {
            throw new InvalidOperationException($"No {nameof(Bullet)}.");
        }

        Bullet bullet = _bullets.Dequeue();
        bullet.SetStartPosition(_pointShot);
        bullet.SetTargetPosition(block.Position);
        bullet.SetTarget(block);
        ModelAdded?.Invoke(bullet);
    }

    private void ChargeMagazine(int amountBullets)
    {
        for (int i = 0; i < amountBullets; i++)
        {
            _bullets.Enqueue(_bulletFactory.Create());
        }
    }
}