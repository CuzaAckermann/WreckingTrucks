using System;

public interface IAmountChangedNotifier
{
    public event Action<float> AmountChanged;

    public float CurrentAmount { get; }

    public int GetMaxAmount();
}