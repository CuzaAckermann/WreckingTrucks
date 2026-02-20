using System;
using UnityEngine;

public class PlanePresenterFactory : PresenterFactory<PlanePresenter>
{
    public PlanePresenterFactory(PresenterFactorySettings<PlanePresenter> factorySettings,
                                 Transform poolParent,
                                 Func<Presenter, Transform, Presenter> createFunction)
                          : base(factorySettings,
                                 poolParent,
                                 createFunction)
    {

    }
}