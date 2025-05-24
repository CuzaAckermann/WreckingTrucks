using System;
using UnityEngine;

public abstract class PresenterFactory<T> : MonoBehaviour, IFactory<T> where T : Presenter
{
    [SerializeField] private T _prefab;

    [Header("Settings Pool")]
    [SerializeField, Min(1)] private int _initialPoolSize = 100;
    [SerializeField, Min(1)] private int _maxPoolSize = 500;
    [SerializeField] private Transform _poolParent;

    private Pool<T> _presenterPool;
    private bool _isInitialized;

    public void Initialize()
    {
        if (_isInitialized)
        {
            throw new InvalidOperationException($"{nameof(PresenterFactory<T>)} is initialized");
        }

        if (_prefab == null)
        {
            throw new NullReferenceException($"{nameof(_prefab)} is not assigned");
        }

        if (_poolParent == null)
        {
            throw new NullReferenceException($"{nameof(_poolParent)} is not assigned");
        }

        _presenterPool = new Pool<T>(CreatePresenter,
                                     ActivatePresenter,
                                     DeactivatePresenter,
                                     DestroyPresenter,
                                     _initialPoolSize,
                                     _maxPoolSize);

        _isInitialized = true;
    }

    public T Create()
    {
        if (_isInitialized == false)
        {
            throw new InvalidOperationException($"Call {nameof(Initialize)} first");
        }

        return _presenterPool.GetElement();
    }

    private void OnDestroy()
    {
        _presenterPool?.Clear();
    }

    #region Pool Callback
    private T CreatePresenter()
    {
        return Instantiate(_prefab, _poolParent);
    }

    private void ActivatePresenter(T presenter)
    {
        presenter.gameObject.SetActive(true);
        presenter.LifeTimeFinished += OnLifeTimeFinished;
    }

    private void DeactivatePresenter(T presenter)
    {
        presenter.LifeTimeFinished -= OnLifeTimeFinished;
        presenter.ResetState();
        presenter.gameObject.SetActive(false);
    }

    private void DestroyPresenter(T presenter)
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
            _presenterPool.Release((T)presenter);
        }
    }
    #endregion
}