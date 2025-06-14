using System;
using UnityEngine;

public abstract class Presenter : MonoBehaviour, IPresenter
{
    private Transform _transform;
    private bool _isBinded;

    public event Action<Presenter> LifeTimeFinished;

    public Model Model { get; private set; }

    public void Bind(Model model)
    {
        UnsubscribeFromModel();

        Model = model ?? throw new ArgumentNullException(nameof(model));
        _transform = transform;
        _isBinded = true;

        SubscribeToModel();
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void OnDestroy()
    {
        ResetState();
    }

    protected virtual void Subscribe()
    {
        SubscribeToModel();
    }

    protected virtual void Unsubscribe()
    {
        UnsubscribeFromModel();
    }

    private void ResetState()
    {
        UnsubscribeFromModel();
        Model = null;
        _isBinded = false;
    }

    private void SubscribeToModel()
    {
        if (_isBinded)
        {
            Model.PositionChanged += OnPositionChanged;
            Model.RotationChanged += OnRotationChanged;
            Model.Destroyed += OnDestroyed;
            UpdateTransform();
        }
    }

    private void UnsubscribeFromModel()
    {
        if (_isBinded)
        {
            Model.PositionChanged -= OnPositionChanged;
            Model.RotationChanged -= OnRotationChanged;
            Model.Destroyed -= OnDestroyed;
        }
    }

    private void UpdateTransform()
    {
        OnPositionChanged();
        OnRotationChanged();
    }

    private void OnPositionChanged()
    {
        _transform.position = Model.Position;
    }

    private void OnRotationChanged()
    {
        _transform.forward = Model.Forward;
    }

    private void OnDestroyed(Model _)
    {
        LifeTimeFinished?.Invoke(this);
    }
}