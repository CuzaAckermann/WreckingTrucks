using System;

public class GunFactory : ModelFactory<Gun>
{
    private readonly BulletFactory _bulletFactory;
    private readonly GunSettings _gunSettings;
    private readonly StopwatchCreator _stopwatchCreator;
    private readonly GunnerFactory _gunnerFactory;

    public GunFactory(BulletFactory bulletFactory,
                      GunFactorySettings factorySettings,
                      StopwatchCreator stopwatchCreator,
                      GunnerFactory gunnerFactory)
               : base(factorySettings, factorySettings.GunSettings)
    {
        _bulletFactory = bulletFactory ?? throw new ArgumentNullException(nameof(bulletFactory));
        _stopwatchCreator = stopwatchCreator ?? throw new ArgumentNullException(nameof(stopwatchCreator));
        _gunSettings = factorySettings.GunSettings ?? throw new ArgumentNullException(nameof(factorySettings.GunSettings));
        _gunnerFactory = gunnerFactory ?? throw new ArgumentNullException(nameof(gunnerFactory));

        InitPool(factorySettings.InitialPoolSize,
                 factorySettings.MaxPoolCapacity);
    }

    public override Gun Create()
    {
        Gun gun = base.Create();

        gun.SetGunner(_gunnerFactory.Create());

        return gun;
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