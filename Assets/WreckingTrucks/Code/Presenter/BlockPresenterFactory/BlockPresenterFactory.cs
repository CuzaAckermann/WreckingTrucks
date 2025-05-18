using System;
using UnityEngine;

public class BlockPresenterFactory : MonoBehaviour
{
    [SerializeField] private BlockPresenter _prefab;

    [Header("Settings Pool")]
    [SerializeField, Min(1)] private int _initialPoolSize = 100;
    [SerializeField, Min(1)] private int _maxPoolSize = 500;
    [SerializeField] private Transform _poolParent;

    private Pool<BlockPresenter> _pool;
    private bool _isInitialized;

    public void Initialize()
    {
        if (_isInitialized)
        {
            throw new InvalidOperationException($"{nameof(BlockPresenterFactory)} is initialized");
        }

        if (_prefab == null)
        {
            throw new NullReferenceException($"{nameof(_prefab)} is not assigned");
        }

        if (_poolParent == null)
        {
            throw new NullReferenceException($"{nameof(_poolParent)} is not assigned");
        }

        _pool = new Pool<BlockPresenter>(CreatePresenter,
                                         ActivatePresenter,
                                         DeactivatePresenter,
                                         DestroyPresenter,
                                         _initialPoolSize,
                                         _maxPoolSize);

        _isInitialized = true;
    }

    public BlockPresenter GetPresenter()
    {
        if (_isInitialized == false)
        {
            throw new InvalidOperationException($"Call {nameof(Initialize)} first");
        }

        return _pool.GetElement();
    }

    private void OnDestroy()
    {
        _pool?.Clear();
    }

    #region Pool Callback
    private BlockPresenter CreatePresenter()
    {
        return Instantiate(_prefab, _poolParent);
    }

    private void ActivatePresenter(BlockPresenter presenter)
    {
        presenter.gameObject.SetActive(true);
        presenter.LifeTimeFinished += OnLifeTimeFinished;
    }

    private void DeactivatePresenter(BlockPresenter presenter)
    {
        presenter.LifeTimeFinished -= OnLifeTimeFinished;
        presenter.ResetState();
        presenter.gameObject.SetActive(false);
    }

    private void DestroyPresenter(BlockPresenter presenter)
    {
        if (presenter != null)
        {
            presenter.LifeTimeFinished -= OnLifeTimeFinished;
            Destroy(presenter.gameObject);
        }
    }

    private void OnLifeTimeFinished(BlockPresenter presenter)
    {
        if (presenter != null)
        {
            _pool.Release(presenter);
        }
    }
    #endregion
}