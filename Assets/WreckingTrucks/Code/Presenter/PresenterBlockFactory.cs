using System;
using System.Collections.Generic;
using UnityEngine;

public class PresenterBlockFactory : MonoBehaviour
{
    [SerializeField] private BlockPresenter _blockPresenterPrefab;
    [SerializeField, Min(1)] private int _startAmountPrefabs = 100;
    [SerializeField] private Transform _poolParent;

    private Stack<BlockPresenter> _blocksPresentersPool;
    private bool _isInitialized;

    public void Init()
    {
        if (_blockPresenterPrefab == null)
        {
            throw new ArgumentNullException(nameof(_blockPresenterPrefab));
        }

        if (_isInitialized)
        {
            return;
        }

        _blocksPresentersPool = new Stack<BlockPresenter>(_startAmountPrefabs);
        PrewarmPool();

        _isInitialized = true;
    }

    private void OnDestroy()
    {
        if (_blocksPresentersPool == null)
        {
            return;
        }

        foreach (var blockPresenter in _blocksPresentersPool)
        {
            if (blockPresenter != null)
            {
                blockPresenter.LifeTimeFinished -= TakeBlockPresenter;
            }
        }
    }

    public BlockPresenter GetBlockPresenter()
    {
        if (_isInitialized == false)
        {
            throw new InvalidOperationException($"Factory not initialized. Call {nameof(Init)} first.");
        }

        if (_blocksPresentersPool.Count == 0)
        {
            CreateBlockPresenter();
        }

        BlockPresenter blockPresenter = _blocksPresentersPool.Pop();
        blockPresenter.gameObject.SetActive(true);
        blockPresenter.LifeTimeFinished += TakeBlockPresenter;

        return blockPresenter;
    }

    private void TakeBlockPresenter(BlockPresenter blockPresenter)
    {
        if (blockPresenter == null)
        {
            return;
        }

        blockPresenter.LifeTimeFinished -= TakeBlockPresenter;
        blockPresenter.ResetState();
        blockPresenter.gameObject.SetActive(false);
        _blocksPresentersPool.Push(blockPresenter);
    }

    private void PrewarmPool()
    {
        for (int i = 0;  i < _startAmountPrefabs; i++)
        {
            CreateBlockPresenter();
        }
    }

    private void CreateBlockPresenter()
    {
        BlockPresenter blockPresenter = Instantiate(_blockPresenterPrefab, _poolParent);
        blockPresenter.gameObject.SetActive(false);
        _blocksPresentersPool.Push(blockPresenter);
    }
}