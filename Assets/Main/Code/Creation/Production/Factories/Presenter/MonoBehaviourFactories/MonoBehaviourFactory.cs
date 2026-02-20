using System;
using UnityEngine;

public abstract class MonoBehaviourFactory<P> : Factory where P : Presenter
{
    private readonly P _prefab;
    private readonly Transform _poolParent;

    private readonly Func<Presenter, Transform, Presenter> _instantiate;

    public MonoBehaviourFactory(PresenterFactorySettings<P> factorySettings,
                                Transform poolParent,
                                Func<Presenter, Transform, Presenter> createFunction) : base(factorySettings)
    {
        Validator.ValidateNotNull(poolParent, createFunction);

        _prefab = factorySettings.Prefab;
        _poolParent = poolParent;
        _instantiate = createFunction;
    }

    protected override IDestroyable CreateElement()
    {
        P presenter = (P)_instantiate(_prefab, _poolParent);

        presenter.Init();

        return presenter;
    }

    protected override void SubscribeToElement(IDestroyable element)
    {
        P presenter = (P)element;

        presenter.On();

        base.SubscribeToElement(element);
    }

    protected override void UnsubscribeFromElement(IDestroyable element)
    {
        base.UnsubscribeFromElement(element);

        P presenter = (P)element;

        presenter.Off();
    }
}