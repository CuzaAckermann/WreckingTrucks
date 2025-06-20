using System;

public class Charger
{
    private readonly BulletFactory _bulletFactory;

    private int _gunCapacity;

    public Charger(BulletFactory bulletFactory, int gunCapacity)
    {
        if (gunCapacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(gunCapacity));
        }

        _bulletFactory = bulletFactory ?? throw new ArgumentNullException(nameof(bulletFactory));
        _gunCapacity = gunCapacity;
    }

    public void ChargeGun(Gun gun)
    {
        for (int i = 0; i < _gunCapacity; i++)
        {
            gun.PutBullet(_bulletFactory.Create());
        }
    }
}