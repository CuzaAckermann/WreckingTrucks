using System;
using UnityEngine;

public abstract class BlockPresenter : MonoBehaviour
{
    [SerializeField] private Block _model;
    
    private Transform _transform;
    private bool _isInitialized;

    public event Action<BlockPresenter> LifeTimeFinished;

    public void Initialize(Block model)
    {
        if (_isInitialized)
        {
            UnsubscribeFromModel();
        }

        _model = model ?? throw new ArgumentNullException(nameof(model));
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
        _model = null;
        _isInitialized = false;
    }

    public void Destroy()
    {
        _model.Destroy();
    }

    private void SubscribeToModel()
    {
        if (_model != null)
        {
            _model.PositionChanged += OnPositionChanged;
            _model.Destroyed += OnDestroyed;
        }
    }

    private void UnsubscribeFromModel()
    {
        if (_model != null)
        {
            _model.PositionChanged -= OnPositionChanged;
            _model.Destroyed -= OnDestroyed;
        }
    }

    private void OnPositionChanged()
    {
        if (_model != null)
        {
            _transform.position = _model.Position;
        }
    }

    private void OnDestroyed(Block block)
    {
        LifeTimeFinished?.Invoke(this);
    }
}