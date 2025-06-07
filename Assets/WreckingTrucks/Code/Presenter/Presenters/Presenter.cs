using System;
using UnityEngine;

public abstract class Presenter : MonoBehaviour
{
    private Model _model;
    private Transform _transform;
    private bool _isBinded;

    public event Action<Presenter> LifeTimeFinished;

    public void Bind(Model model)
    {
        if (_isBinded)
        {
            UnsubscribeFromModel();
        }

        _model = model ?? throw new ArgumentNullException(nameof(model));
        _transform = transform;
        _transform.forward = _model.Forward;
        _isBinded = true;
        OnPositionChanged();

        SubscribeToModel();
    }

    private void OnEnable()
    {
        if (_isBinded)
        {
            SubscribeToModel();
            OnPositionChanged();
        }
    }

    private void OnDisable()
    {
        if (_isBinded)
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
        _isBinded = false;
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

    private void OnDestroyed(Model model)
    {
        LifeTimeFinished?.Invoke(this);
    }
}