using System;
using UnityEngine;
using UnityEngine.UI;

public class SmoothingAmountDisplay : MonoBehaviour, ITickable
{
    [SerializeField] private Slider _slider;
    [SerializeField] private int _amountOfChange;

    private IAmountChangedNotifier _notifier;
    private TickEngine _tickEngine;
    private bool _isSubscribe;
    private bool _isActivated;

    private int _targetValue;

    public void Initialize(TickEngine tickEngine)
    {
        _tickEngine = tickEngine ?? throw new ArgumentNullException(nameof(tickEngine));
    }

    public void Initialize(IAmountChangedNotifier notifier)
    {
        _notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));

        _slider.wholeNumbers = true;
        _slider.maxValue = _notifier.GetMaxAmount();

        SubscribeToNotifier();
    }

    private void OnEnable()
    {
        SubscribeToNotifier();
    }

    private void OnDisable()
    {
        UnsubscribeFromNotifier();
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
        int direction = _targetValue - (int)_slider.value;
        int amountOfChange;

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
            _notifier.AmountChanged += OnCurrentAmountChanged;
            _isSubscribe = true;
        }
    }

    private void UnsubscribeFromNotifier()
    {
        if (_notifier != null && _isSubscribe)
        {
            _notifier.AmountChanged -= OnCurrentAmountChanged;
            _isSubscribe = false;
        }
    }

    private void Activate()
    {
        if (_isActivated == false)
        {
            _tickEngine.AddTickable(this);
            _isActivated = true;
        }
    }

    private void Deactivate()
    {
        if (_isActivated)
        {
            _tickEngine.RemoveTickable(this);
            _isActivated = false;
        }
    }

    private void OnMaxAmountChanged(int amount)
    {

    }

    private void OnCurrentAmountChanged(int amount)
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