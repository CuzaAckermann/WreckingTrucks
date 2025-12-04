using System;

public class GunFactory : ModelFactory<Gun>
{
    private readonly BulletFactory _bulletFactory;
    private readonly GunSettings _gunSettings;
    private readonly StopwatchCreator _stopwatchCreator;

    public GunFactory(BulletFactory bulletFactory, GunFactorySettings factorySettings, StopwatchCreator stopwatchCreator)
               : base(factorySettings, factorySettings.GunSettings)
    {
        _bulletFactory = bulletFactory ?? throw new ArgumentNullException(nameof(bulletFactory));
        _stopwatchCreator = stopwatchCreator ?? throw new ArgumentNullException(nameof(stopwatchCreator));
        _gunSettings = factorySettings.GunSettings ?? throw new ArgumentNullException(nameof(factorySettings.GunSettings));

        InitPool(factorySettings.InitialPoolSize,
                 factorySettings.MaxPoolCapacity);
    }

    protected override Gun CreateElement()
    {
        return new Gun(ModelSettings.Movespeed,
                       ModelSettings.Rotatespeed,
                       _bulletFactory,
                       _gunSettings.Capacity,
                       _stopwatchCreator.Create(),
                       _gunSettings.ShotCooldown);
    }
}