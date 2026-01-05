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
        UnsubscribeFromNotifier();

        _notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));

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
        gameObject.SetActive(false);

        UnsubscribeFromNotifier();
    }

    protected virtual string ConvertAmount(float amount)
    {
        return amount.ToString();
    }

    private void SubscribeToNotifier()
    {
        if (_notifier != null && _isSubscribed == false)
        {
            _notifier.AmountChanged += OnAmountChanged;
            OnAmountChanged(_notifier.CurrentAmount);

            _isSubscribed = true;
        }
    }

    private void UnsubscribeFromNotifier()
    {
        if (_isSubscribed)
        {
            _notifier.AmountChanged -= OnAmountChanged;

            _isSubscribed = false;
        }
    }

    private void OnAmountChanged(float amount)
    {
        _textAmount.text = ConvertAmount(amount);
    }
}