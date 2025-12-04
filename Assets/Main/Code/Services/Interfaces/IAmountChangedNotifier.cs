using System;

public interface IAmountChangedNotifier
{
    public event Action<float> AmountChanged;

    public int GetMaxAmount();
}