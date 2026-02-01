using System;
using UnityEngine;

public class DeltaTimeCoefficientDefiner : IAmountChangedNotifier
{
    private readonly BackgroundInput _backgroundInput;

    public DeltaTimeCoefficientDefiner(BackgroundInput backgroundInput)
    {
        _backgroundInput = backgroundInput ?? throw new ArgumentNullException(nameof(backgroundInput));

        CurrentAmount = 1;

        SubscribeToBackgroundInput();
    }

    public event Action<float> AmountChanged;

    public float CurrentAmount { get; private set; }

    public int GetMaxAmount()
    {
        throw new NotImplementedException();
    }

    private void SubscribeToBackgroundInput()
    {
        _backgroundInput.TimeCoefficientInstalled += Set;
        _backgroundInput.TimeCoefficientIncreased += Increase;
        _backgroundInput.TimeCoefficientDecreased += Decrease;
    }

    private void UnsubscribeFromBackgroundInput()
    {
        _backgroundInput.TimeCoefficientInstalled -= Set;
        _backgroundInput.TimeCoefficientIncreased -= Increase;
        _backgroundInput.TimeCoefficientDecreased -= Decrease;
    }

    private void Increase(float magnificationValue)
    {
        Set(CurrentAmount + magnificationValue);
    }

    private void Decrease(float reductionValue)
    {
        Set(CurrentAmount - reductionValue);
    }

    private void Set(float deltaTimeCoefficient)
    {
        CurrentAmount = Mathf.Max(0, deltaTimeCoefficient);
        AmountChanged?.Invoke(CurrentAmount);
    }
}