using System;
using UnityEngine;

public class BulletPresenterFactory : PresenterFactory<BulletPresenter>
{
    public BulletPresenterFactory(PresenterFactorySettings<BulletPresenter> factorySettings,
                                  Transform poolParent,
                                  Func<Presenter, Transform, Presenter> createFunction)
                           : base(factorySettings,
                                  poolParent,
                                  createFunction)
    {

    }
}