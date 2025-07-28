using System;

public interface IAmountChangedNotifier
{
    public event Action<int> AmountChanged;
}