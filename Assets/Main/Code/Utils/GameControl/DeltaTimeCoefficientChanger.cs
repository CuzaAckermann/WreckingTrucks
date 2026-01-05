using System;
using System.Collections.Generic;
using UnityEngine;

public class DeltaTimeCoefficientDefiner : IAmountChangedNotifier
{
    private readonly List<TimeButton> _timeButtons;

    private readonly TimeButton _decreasedTimeButton;
    private readonly TimeButton _increasedTimeButton;

    public DeltaTimeCoefficientDefiner(List<TimeButton> timeButtons,
                                       TimeButton decreasedTimeButton,
                                       TimeButton increasedTimeButton)
    {
        if (timeButtons == null)
        {
            throw new ArgumentNullException(nameof(timeButtons));
        }

        if (timeButtons.Count == 0)
        {
            throw new ArgumentException($"{nameof(timeButtons)} is empty");
        }

        _timeButtons = timeButtons ?? throw new ArgumentNullException(nameof(timeButtons));
        CurrentAmount = _timeButtons[2].TimeCoefficient;

        _decreasedTimeButton = decreasedTimeButton ?? throw new ArgumentNullException(nameof(decreasedTimeButton));
        _increasedTimeButton = increasedTimeButton ?? throw new ArgumentNullException(nameof(increasedTimeButton));
    }

    public event Action<float> AmountChanged;

    public float CurrentAmount { get; private set; }

    public int GetMaxAmount()
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        for (int i = 0; i < _timeButtons.Count; i++)
        {
            if (Input.GetKeyDown(_timeButtons[i].Button))
            {
                SetDeltaTimeCoefficient(_timeButtons[i].TimeCoefficient);

                return;
            }
        }

        if (Input.GetKey(_decreasedTimeButton.Button))
        {
            SetDeltaTimeCoefficient(CurrentAmount - _decreasedTimeButton.TimeCoefficient);
        }

        if (Input.GetKey(_increasedTimeButton.Button))
        {
            SetDeltaTimeCoefficient(CurrentAmount + _increasedTimeButton.TimeCoefficient);
        }
    }

    private void SetDeltaTimeCoefficient(float deltaTimeCoefficient)
    {
        CurrentAmount = Mathf.Max(0, deltaTimeCoefficient);
        AmountChanged?.Invoke(CurrentAmount);
    }
}