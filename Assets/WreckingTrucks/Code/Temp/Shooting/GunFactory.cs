using System;
using System.Collections.Generic;

public class GunFactory : ModelFactory<Gun>
{
    private readonly BulletFactory _bulletFactory;

    private int _gunCapacity;

    public GunFactory(int initialPoolSize,
                      int maxPoolCapacity,
                      BulletFactory bulletFactory,
                      int gunCapacity)
    {
        if (gunCapacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(gunCapacity));
        }

        _bulletFactory = bulletFactory ?? throw new ArgumentNullException(nameof(bulletFactory));
        _gunCapacity = gunCapacity;
        InitializePool(initialPoolSize, maxPoolCapacity);
    }

    protected override Gun CreateModel()
    {
        return new Gun(CreateBullets());
    }

    private Queue<Bullet> CreateBullets()
    {
        Queue<Bullet> bullet = new Queue<Bullet>();

        for (int i = 0; i < _gunCapacity; i++)
        {
            bullet.Enqueue(_bulletFactory.Create());
        }

        return bullet;
    }
}