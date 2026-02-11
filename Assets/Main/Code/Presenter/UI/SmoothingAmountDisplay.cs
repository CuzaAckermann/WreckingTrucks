using System;
using UnityEngine;
using UnityEngine.UI;

public class SmoothingAmountDisplay : MonoBehaviour, ITickable
{
    [SerializeField] private Slider _slider;
    [SerializeField] private int _amountOfChange;

    private IClampedAmount _notifier;
    private float _targetValue;

    private bool _isSubscribe;
    private bool _isActivated;

    public void Init(IClampedAmount notifier)
    {
        _notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));

        _slider.wholeNumbers = true;
        _slider.maxValue = _notifier.Max.Value;

        SubscribeToNotifier();
    }

    public event Action<IDestroyable> Destroyed;

    public event Action<ITickable> Activated;

    public event Action<ITickable> Deactivated;

    private void OnEnable()
    {
        SubscribeToNotifier();
    }

    private void OnDisable()
    {
        UnsubscribeFromNotifier();
    }

    private void OnDestroy()
    {
        Destroy();
    }

    public void Destroy()
    {
        Destroyed?.Invoke(this);
    }

    public void On()
    {
        gameObject.SetActive(true);
        SubscribeToNotifier();
    }

    public void Off()
    {
        _slider.value = 0;
        gameObject.SetActive(false);
        UnsubscribeFromNotifier();
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

    private void SubscribeToNotifier()
    {
        if (_notifier != null && _isSubscribe == false)
        {
            _notifier.ValueChanged += OnCurrentAmountChanged;
            _isSubscribe = true;
        }
    }

    private void UnsubscribeFromNotifier()
    {
        if (_notifier != null && _isSubscribe)
        {
            _notifier.ValueChanged -= OnCurrentAmountChanged;
            _isSubscribe = false;
        }
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