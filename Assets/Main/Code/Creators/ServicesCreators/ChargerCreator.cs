using System;

public class ChargerCreator
{
    private readonly BulletFactory _bulletFactory;

    public ChargerCreator(BulletFactory bulletFactory)
    {
        _bulletFactory = bulletFactory ?? throw new ArgumentNullException(nameof(bulletFactory));
    }

    public Charger Create()
    {
        return new Charger(_bulletFactory);
    }
}