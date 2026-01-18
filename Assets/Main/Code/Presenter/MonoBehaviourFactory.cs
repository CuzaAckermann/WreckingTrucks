using System;
using UnityEngine;

public abstract class MonoBehaviourFactory<C> : MonoBehaviour where C : Presenter
{
    [SerializeField] private Transform _poolParent;

    protected EventBus EventBus;
    
    private C _prefab;
    private Pool<C> _poolOfModel;

    private bool _isInitialized;

    public void Init(PresenterFactorySettings<C> factorySettings, EventBus eventBus)
    {
        if (_isInitialized)
        {
            throw new InvalidOperationException($"{nameof(MonoBehaviourFactory<C>)} is initialized");
        }

        if (factorySettings == null)
        {
            throw new ArgumentNullException(nameof(factorySettings));
        }

        if (_poolParent == null)
        {
            throw new NullReferenceException($"{nameof(_poolParent)} is not assigned");
        }

        _prefab = factorySettings.Prefab;

        _poolOfModel = new Pool<C>(CreateElement,
                                   ActivateCreatable,
                                   DeactivateCreatable,
                                   DestroyCreatable,
                                   factorySettings.InitialPoolSize,
                                   factorySettings.MaxPoolCapacity);

        EventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        _isInitialized = true;
    }

    private void OnDestroy()
    {
        _poolOfModel?.Clear();
    }

    public C Create()
    {
        if (_isInitialized == false)
        {
            throw new InvalidOperationException($"Call {nameof(Init)} first");
        }

        C presenter = _poolOfModel.GetElement();

        EventBus.Invoke(new PresenterCreatedSignal<C>(presenter));

        return _poolOfModel.GetElement();
    }

    private C CreateElement()
    {
        C creatable = Instantiate(_prefab, _poolParent);

        creatable.Init();

        return creatable;
    }

    private void ActivateCreatable(C creatable)
    {
        creatable.gameObject.SetActive(true);
        creatable.LifeTimeFinished += OnLifeTimeFinished;
    }

    private void DeactivateCreatable(C creatable)
    {
        creatable.LifeTimeFinished -= OnLifeTimeFinished;
        creatable.gameObject.SetActive(false);
    }

    private void DestroyCreatable(C creatable)
    {
        creatable.LifeTimeFinished -= OnLifeTimeFinished;
        Destroy(creatable.gameObject);
    }

    private void OnLifeTimeFinished(Creatable creatable)
    {
        _poolOfModel.Release((C)creatable);
    }
}
