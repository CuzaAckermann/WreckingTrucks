using System;

public class Charger
{
    private readonly BulletFactory _bulletFactory;

    public Charger(BulletFactory bulletFactory)
    {
        _bulletFactory = bulletFactory ?? throw new ArgumentNullException(nameof(bulletFactory));
    }

    public void ChargeGun(Gun gun)
    {
        for (int i = 0; i < gun.Capacity; i++)
        {
            gun.PutBullet(_bulletFactory.Create());
        }
    }
}