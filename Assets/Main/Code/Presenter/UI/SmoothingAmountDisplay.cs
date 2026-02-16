using System;
using UnityEngine;
using UnityEngine.UI;

public class SmoothingAmountDisplay : MonoBehaviourSubscriber, ITickable
{
    [SerializeField] private Slider _slider;
    [SerializeField] private int _amountOfChange;

    private IClampedAmount _notifier;
    private float _targetValue;

    private bool _isActivated;

    public void Init(IClampedAmount notifier)
    {
        _notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));

        _slider.wholeNumbers = true;
        _slider.maxValue = _notifier.Max.Value;

        Init();
    }

    public event Action<IDestroyable> Destroyed;

    public event Action<ITickable> Activated;

    public event Action<ITickable> Deactivated;

    private void OnDestroy()
    {
        Destroy();
    }

    public void Destroy()
    {
        Destroyed?.Invoke(this);
    }

    public void Tick(float deltaTime)
    {
        float direction = _targetValue - _slider.value;
        float amountOfChange;

        if (direction < 0)
        {
            amountOfChange = Mathf.Max(-_amountOfChange, direction);
        }
        else
        {
            amountOfChange = Mathf.Min(_amountOfChange, direction);
        }

        if (_targetValue - _slider.value > amountOfChange)
        {
            amountOfChange *= 2;
        }

        _slider.value += amountOfChange;

        if (direction == 0)
        {
            Deactivate();
        }
    }

    protected override void Subscribe()
    {
        _notifier.ValueChanged += OnCurrentAmountChanged;
    }

    protected override void Unsubscribe()
    {
        _notifier.ValueChanged -= OnCurrentAmountChanged;
    }

    private void Activate()
    {
        //if (_isActivated == false)
        //{
        //    Activated?.Invoke(this);

        //    _isActivated = true;
        //}
    }

    private void Deactivate()
    {
        //if (_isActivated)
        //{
        //    Deactivated?.Invoke(this);

        //    _isActivated = false;
        //}
    }

    private void OnMaxAmountChanged(int amount)
    {

    }

    private void OnCurrentAmountChanged(float amount)
    {
        //_direction = amount - (int)_slider.value;

        //if (_direction != 0)
        //{
        //    Activate();
        //}

        _targetValue = amount;

        if (_targetValue != (int)_slider.value)
        {
            Activate();
        }
    }
}