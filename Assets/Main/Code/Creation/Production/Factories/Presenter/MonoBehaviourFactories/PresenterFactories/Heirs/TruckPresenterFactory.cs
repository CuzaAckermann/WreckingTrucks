using System;
using UnityEngine;

public class TruckPresenterFactory : PresenterFactory<TruckPresenter>
{
    public TruckPresenterFactory(PresenterFactorySettings<TruckPresenter> factorySettings,
                                 Transform poolParent,
                                 Func<Presenter, Transform, Presenter> createFunction)
                          : base(factorySettings,
                                 poolParent,
                                 createFunction)
    {

    }
}