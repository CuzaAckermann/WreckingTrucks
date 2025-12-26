using System;
using System.Collections.Generic;
using UnityEngine;

public class BarrelFactory : ModelFactory<Barrel>
{
    public BarrelFactory(FactorySettings factorySettings,
                         ModelSettings modelSettings)
                  : base(factorySettings,
                         modelSettings)
    {
        InitPool(factorySettings.InitialPoolSize,
                 factorySettings.MaxPoolCapacity);
    }

    protected override Barrel CreateElement()
    {
        return new Barrel(ModelSettings.Movespeed,
                          ModelSettings.Rotatespeed);
    }
}