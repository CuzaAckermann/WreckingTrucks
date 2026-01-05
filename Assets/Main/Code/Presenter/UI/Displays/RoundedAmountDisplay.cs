using UnityEngine;

public class RoundedAmountDisplay : AmountDisplay
{
    [SerializeField] private int _digitsAfterDecimal = 2;

    protected override string ConvertAmount(float amount)
    {
        return amount.ToString($"F{_digitsAfterDecimal}");
    }
}