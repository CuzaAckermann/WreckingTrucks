using System;
using System.Collections.Generic;
using UnityEngine;

public class GunnerFactory : ModelFactory<Gunner>
{
    private readonly TurretFactory _turretFactory;

    public GunnerFactory(FactorySettings factorySettings,
                         ModelSettings modelSettings,
                         TurretFactory turretFactory)
                  : base(factorySettings,
                         modelSettings)
    {
        _turretFactory = turretFactory ?? throw new ArgumentNullException(nameof(turretFactory));

        InitPool(factorySettings.InitialPoolSize,
                 factorySettings.MaxPoolCapacity);
    }

    public override Gunner Create()
    {
        Gunner gunner = base.Create();

        gunner.Prepare(_turretFactory.Create());

        return gunner;
    }

    protected override Gunner CreateElement()
    {
        return new Gunner(ModelSettings.Movespeed,
                          ModelSettings.Rotatespeed);
    }
}