using System;
using UnityEngine;

public abstract class PresenterFactory : MonoBehaviour
{
    [Header("Settings Pool")]
    [SerializeField, Min(1)] private int _initialPoolSize = 100;
    [SerializeField, Min(1)] private int _maxPoolSize = 500;
    [SerializeField] private Transform _poolParent;

    private Presenter _prefab;
    private Pool<Presenter> _presenterPool;
    private bool _isInitialized;

    public void Initialize()
    {
        if (_isInitialized)
        {
            throw new InvalidOperationException($"{nameof(PresenterFactory)} is initialized");
        }

        _prefab = GetPresenterPrefab();

        if (_prefab == null)
        {
            throw new NullReferenceException($"{nameof(_prefab)} is not assigned");
        }

        if (_poolParent == null)
        {
            throw new NullReferenceException($"{nameof(_poolParent)} is not assigned");
        }

        _presenterPool = new Pool<Presenter>(CreatePresenter,
                                             ActivatePresenter,
                                             DeactivatePresenter,
                                             DestroyPresenter,
                                             _initialPoolSize,
                                             _maxPoolSize);

        _isInitialized = true;
    }

    private void OnDestroy()
    {
        _presenterPool?.Clear();
    }

    public Presenter Create()
    {
        if (_isInitialized == false)
        {
            throw new InvalidOperationException($"Call {nameof(Initialize)} first");
        }

        return _presenterPool.GetElement();
    }

    protected abstract Presenter GetPresenterPrefab();

    private Presenter CreatePresenter()
    {
        return Instantiate(_prefab, _poolParent);
    }

    private void ActivatePresenter(Presenter presenter)
    {
        presenter.gameObject.SetActive(true);
        presenter.LifeTimeFinished += OnLifeTimeFinished;
    }

    private void DeactivatePresenter(Presenter presenter)
    {
        presenter.LifeTimeFinished -= OnLifeTimeFinished;
        presenter.ResetState();
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