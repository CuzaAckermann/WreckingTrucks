using System;
using UnityEngine;

public class CartrigeBoxPresenterFactory : PresenterFactory<CartrigeBoxPresenter>
{
    public CartrigeBoxPresenterFactory(PresenterFactorySettings<CartrigeBoxPresenter> factorySettings,
                                       Transform poolParent,
                                       Func<Presenter, Transform, Presenter> createFunction)
                                : base(factorySettings,
                                       poolParent,
                                       createFunction)
    {

    }
}