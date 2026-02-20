using System;
using UnityEngine;

public class SmoothValueFollower : IAmount, ITickable
{
    private readonly float _speed;

    private IAmount _target;
    private float _value;

    private bool _isActivated;

    public SmoothValueFollower(float speed, float value = 0)
    {
        Validator.ValidateMin(speed, 0, true);
        Validator.ValidateMin(value, 0, false);

        _speed = speed;
        _value = value;
    }

    public float Value => _value;

    public event Action<float> Changed;

    public event Action<ITickable> Activated;
    public event Action<ITickable> Deactivated;
    public event Action<IDestroyable> Destroyed;

    public void Destroy()
    {
        _target.Changed -= OnCurrentAmountChanged;

        Destroyed?.Invoke(this);
    }

    public void SetTarget(IAmount target)
    {
        Validator.ValidateNotNull(target);

        _target = target;

        _target.Changed += OnCurrentAmountChanged;
        UpdateValue(_target.Value);
    }

    public void Tick(float deltaTime)
    {
        UpdateValue(Mathf.MoveTowards(_value, _target.Value, _speed * deltaTime));

        if (Mathf.Approximately(_value, _target.Value) == false)
        {
            return;
        }

        Deactivate();
    }

    private void UpdateValue(float newValue)
    {
        _value = newValue;

        Changed?.Invoke(newValue);
    }

    private void Activate()
    {
        _isActivated = true;

        Activated?.Invoke(this);
    }

    private void Deactivate()
    {
        _isActivated = false;

        Deactivated?.Invoke(this);
    }

    private void OnCurrentAmountChanged(float amount)
    {
        if (_isActivated)
        {
            return;
        }

        if (Mathf.Approximately(amount, _value))
        {
            return;
        }

        Activate();
    }
}