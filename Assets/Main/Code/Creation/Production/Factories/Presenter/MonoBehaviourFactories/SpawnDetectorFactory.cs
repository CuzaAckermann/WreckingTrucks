using System;
using UnityEngine;

public class SpawnDetectorFactory : PresenterFactory<SpawnDetector>
{
    public SpawnDetectorFactory(PresenterFactorySettings<SpawnDetector> factorySettings,
                                Transform poolParent,
                                Func<Presenter, Transform, Presenter> createFunction)
                         : base(factorySettings,
                                poolParent,
                                createFunction)
    {

    }
}