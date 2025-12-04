using System;
using UnityEngine;
using TMPro;

public class AmountDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _textAmount;

    private IAmountChangedNotifier _notifier;
    private bool _isSubscribed = false;

    public void Init(IAmountChangedNotifier notifier)
    {
        if (_notifier != null)
        {
            _notifier.AmountChanged -= OnAmountChanged;
        }

        _notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));

        _isSubscribed = true;
    }

    private void OnEnable()
    {
        if (_notifier != null && _isSubscribed == false)
        {
            SubscribeToNotifier();
            _isSubscribed = true;
        }
    }

    private void OnDisable()
    {
        if (_notifier != null && _isSubscribed)
        {
            UnsubscribeFromNotifier();
            _isSubscribed = false;
        }
    }

    public void On()
    {
        gameObject.SetActive(true);
        
        SubscribeToNotifier();
    }

    public void Off()
    {
        gameObject.SetActive(false);

        if (_notifier != null)
        {
            UnsubscribeFromNotifier();
        }
    }

    private void SubscribeToNotifier()
    {
        _notifier.AmountChanged += OnAmountChanged;
    }

    private void UnsubscribeFromNotifier()
    {
        _notifier.AmountChanged -= OnAmountChanged;
    }

    private void OnAmountChanged(float amount)
    {
        _textAmount.text = amount.ToString("F2");
    }
}