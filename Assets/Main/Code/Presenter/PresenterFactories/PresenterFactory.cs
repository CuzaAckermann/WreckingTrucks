using System;
using UnityEngine;

public abstract class PresenterFactory<P> : MonoBehaviour, IPresenterCreator where P : Presenter
{
    [SerializeField] private Transform _poolParent;

    private P _prefab;
    private Pool<Presenter> _presenterPool;
    private bool _isInitialized;

    public void Initialize(PresenterFactorySettings<P> factorySettings)
    {
        if (_isInitialized)
        {
            throw new InvalidOperationException($"{nameof(PresenterFactory<P>)} is initialized");
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
        _presenterPool = new Pool<Presenter>(CreatePresenter,
                                             ActivatePresenter,
                                             DeactivatePresenter,
                                             DestroyPresenter,
                                             factorySettings.InitialPoolSize,
                                             factorySettings.MaxPoolCapacity);
        _isInitialized = true;
    }

    private void OnDestroy()
    {
        _presenterPool?.Clear();
    }

    public IPresenter Create()
    {
        if (_isInitialized == false)
        {
            throw new InvalidOperationException($"Call {nameof(Initialize)} first");
        }

        return _presenterPool.GetElement();
    }

    private Presenter CreatePresenter()
    {
        Presenter presenter = Instantiate(_prefab, _poolParent);
        presenter.InitializeComponents();

        return presenter;
    }

    private void ActivatePresenter(Presenter presenter)
    {
        presenter.gameObject.SetActive(true);
        presenter.LifeTimeFinished += OnLifeTimeFinished;
    }

    private void DeactivatePresenter(Presenter presenter)
    {
        presenter.LifeTimeFinished -= OnLifeTimeFinished;
        presenter.gameObject.SetActive(false);
    }

    private void DestroyPresenter(Presenter presenter)
    {
        if (presenter != null)
        {
            presenter.LifeTimeFinished -= OnLifeTimeFinished;
            Destroy(presenter.gameObject);
        }
    }

    private void OnLifeTimeFinished(Presenter presenter)
    {
        if (presenter != null)
        {
            _presenterPool.Release(presenter);
        }
    }
}