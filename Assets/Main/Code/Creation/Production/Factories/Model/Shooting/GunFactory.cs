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
        Validator.ValidateNotNull(eventBus, bulletFactory, factorySettings.GunSettings, gunnerFactory);

        _eventBus = eventBus;

        _bulletFactory = bulletFactory;
        _gunSettings = factorySettings.GunSettings;
        _gunnerFactory = gunnerFactory;
    }

    public override IDestroyable Create()
    {
        if (Validator.IsRequiredType(base.Create(), out Gun gun) == false)
        {
            throw new InvalidOperationException();
        }

        if (Validator.IsRequiredType(_gunnerFactory.Create(), out Gunner gunner) == false)
        {
            throw new InvalidOperationException();
        }

        _eventBus.Invoke(new CreatedSignal<ICommandCreator>(gun));

        gun.SetGunner(gunner);

        return gun;
    }

    protected override IDestroyable CreateElement()
    {
        Placeable positionManipulator = new Placeable();

        return new Gun(positionManipulator,
                       MoverCreator.Create(positionManipulator),
                       RotatorCreator.Create(positionManipulator),
                       _bulletFactory,
                       new Placer(_eventBus),
                       _gunSettings.Capacity,
                       _gunSettings.ShotCooldown);
    }
}