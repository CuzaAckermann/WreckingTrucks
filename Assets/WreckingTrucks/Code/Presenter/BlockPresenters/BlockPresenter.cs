using System;
using UnityEngine;

public abstract class BlockPresenter : MonoBehaviour
{
    private Block _block;
    private Transform _transform;
    private bool _isInitialized;

    public event Action<BlockPresenter> LifeTimeFinished;

    public void Initialize(Block block)
    {
        if (_isInitialized)
        {
            UnsubscribeFromModel();
        }

        _block = block ?? throw new ArgumentNullException(nameof(block));
        _transform = transform;
        _isInitialized = true;
        OnPositionChanged();

        SubscribeToModel();
    }

    private void OnEnable()
    {
        if (_isInitialized)
        {
            SubscribeToModel();
            OnPositionChanged();
        }
    }

    private void OnDisable()
    {
        if (_isInitialized)
        {
            UnsubscribeFromModel();
        }
    }

    private void OnDestroy()
    {
        ResetState();
    }

    public void ResetState()
    {
        UnsubscribeFromModel();
        _block = null;
        _isInitialized = false;
    }

    public void Destroy()
    {
        _block.Destroy();
    }

    private void SubscribeToModel()
    {
        if (_block != null)
        {
            _block.PositionChanged += OnPositionChanged;
            _block.Destroyed += OnDestroyed;
        }
    }

    private void UnsubscribeFromModel()
    {
        if (_block != null)
        {
            _block.PositionChanged -= OnPositionChanged;
            _block.Destroyed -= OnDestroyed;
        }
    }

    private void OnPositionChanged()
    {
        if (_transform != null && _block != null)
        {
            _transform.position = _block.Position;
        }
    }

    private void OnDestroyed(Block block)
    {
        if (_isInitialized && block == _block)
        {
            LifeTimeFinished?.Invoke(this);
        }
    }
}