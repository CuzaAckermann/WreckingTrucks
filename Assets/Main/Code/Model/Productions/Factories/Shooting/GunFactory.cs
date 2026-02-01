using System;

public class GunFactory : ModelFactory<Gun>
{
    private readonly EventBus _eventBus;

    private readonly BulletFactory _bulletFactory;
    private readonly GunSettings _gunSettings;
    private readonly GunnerFactory _gunnerFactory;

    public GunFactory(EventBus eventBus,
                      BulletFactory bulletFactory,
                      GunFactorySettings factorySettings,
                      GunnerFactory gunnerFactory)
               : base(factorySettings, factorySettings.GunSettings)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        _bulletFactory = bulletFactory ?? throw new ArgumentNullException(nameof(bulletFactory));
        _gunSettings = factorySettings.GunSettings ?? throw new ArgumentNullException(nameof(factorySettings.GunSettings));
        _gunnerFactory = gunnerFactory ?? throw new ArgumentNullException(nameof(gunnerFactory));

        InitPool(factorySettings.InitialPoolSize,
                 factorySettings.MaxPoolCapacity);
    }

    public override Gun Create()
    {
        Gun gun = base.Create();

        //Logger.Log(gun.GetType());
        _eventBus.Invoke(new CreatedSignal<ICommandCreator>(gun));

        gun.SetGunner(_gunnerFactory.Create());

        return gun;
    }

    protected override Gun CreateElement()
    {
        return new Gun(ModelSettings.Movespeed,
                       ModelSettings.Rotatespeed,
                       _bulletFactory,
                       _gunSettings.Capacity,
                       _gunSettings.ShotCooldown);
    }
}