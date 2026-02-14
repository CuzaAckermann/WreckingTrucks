using UnityEngine;
using TMPro;

public class AmountDisplay : Indicator<IAmount>
{
    [SerializeField] private TMP_Text _textAmount;

    protected override void SubscribeToNotifier(IAmount notifier)
    {
        notifier.ValueChanged += OnAmountChanged;
        OnAmountChanged(notifier.Value);
    }

    protected override void UnsubscribeFromNotifier(IAmount notifier)
    {
        notifier.ValueChanged -= OnAmountChanged;
    }

    protected virtual string ConvertAmount(float amount)
    {
        return amount.ToString();
    }

    private void OnAmountChanged(float amount)
    {
        _textAmount.text = ConvertAmount(amount);
    }
}