using System;
using UnityEngine;

public abstract class PresenterFactory<P> : MonoBehaviourFactory<P> where P : Presenter
{
    public PresenterFactory(PresenterFactorySettings<P> factorySettings,
                            Transform poolParent,
                            Func<Presenter, Transform, Presenter> createFunction)
                     : base(factorySettings,
                            poolParent,
                            createFunction)
    {

    }

    public override Type GetCreatableType()
    {
        return typeof(P);
    }
}