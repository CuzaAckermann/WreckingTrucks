using System;
using UnityEngine;
using TMPro;

public class AmountDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _textAmount;

    private IAmount _notifier;
    private bool _isSubscribed = false;

    public void Init(IAmount notifier)
    {
        UnsubscribeFromNotifier();

        _notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));
        OnAmountChanged(_notifier.Value);

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
            _notifier.ValueChanged += OnAmountChanged;
            OnAmountChanged(_notifier.Value);

            _isSubscribed = true;
        }
    }

    private void UnsubscribeFromNotifier()
    {
        if (_isSubscribed)
        {
            _notifier.ValueChanged -= OnAmountChanged;

            _isSubscribed = false;
        }
    }

    private void OnAmountChanged(float amount)
    {
        _textAmount.text = ConvertAmount(amount);
    }
}