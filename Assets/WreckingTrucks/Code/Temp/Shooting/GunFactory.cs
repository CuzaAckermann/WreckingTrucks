using System;

public class GunFactory : ModelFactory<Gun>
{
    private BulletFactory _bulletFactory;
    private int _capacityGun = 100;

    public GunFactory(int initialPoolSize,
                      int maxPoolCapacity,
                      BulletFactory bulletFactory,
                      int capacityGun)
               : base(initialPoolSize,
                      maxPoolCapacity)
    {
        if (capacityGun <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacityGun));
        }

        _bulletFactory = bulletFactory ?? throw new ArgumentNullException(nameof(bulletFactory));
        _capacityGun = capacityGun;
    }

    protected override Gun CreateModel()
    {
        return new Gun(_bulletFactory, _capacityGun);
    }
}