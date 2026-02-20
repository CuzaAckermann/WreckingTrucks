using System;
using UnityEngine;

public class BlockPresenterFactory : PresenterFactory<BlockPresenter>
{
    public BlockPresenterFactory(PresenterFactorySettings<BlockPresenter> factorySettings,
                                 Transform poolParent,
                                 Func<Presenter, Transform, Presenter> createFunction)
                          : base(factorySettings,
                                 poolParent,
                                 createFunction)
    {

    }
}