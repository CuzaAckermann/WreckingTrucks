using UnityEngine;
using TMPro;

public class AmountDisplay : MonoBehaviourSubscriber
{
    [SerializeField] private TMP_Text _textAmount;

    private IAmount _amount;

    public void Init(IAmount amount)
    {
        Validator.ValidateNotNull(amount);

        _amount = amount;

        Init();
    }

    protected override void Subscribe()
    {
        _amount.Changed += OnAmountChanged;
        OnAmountChanged(_amount.Value);
    }

    protected override void Unsubscribe()
    {
        _amount.Changed -= OnAmountChanged;
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