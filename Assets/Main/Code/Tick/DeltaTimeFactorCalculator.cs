using System;
using System.Collections.Generic;

public class DeltaTimeFactorCalculator : IAbility
{
    private readonly IInput _input;
    private readonly ClampedAmount _deltaTimeFactor;
    private readonly Dictionary<ValueButton, Action> _handlers;

    public DeltaTimeFactorCalculator(IInput input,
                                     DeltaTimeFactorSettings settings)
    {
        Validator.ValidateNotNull(input, settings);

        _input = input;
        _deltaTimeFactor = new ClampedAmount(settings.Initial,
                                             settings.Min,
                                             settings.Max);
        _handlers = new Dictionary<ValueButton, Action>();

        for (int currentButton = 0; currentButton < _input.TimeFlowInput.TimeButtons.Count; currentButton++)
        {
            ValueButton button = _input.TimeFlowInput.TimeButtons[currentButton];

            _handlers.Add(button, () => Change(button.Value));
        }

        _handlers.Add(_input.TimeFlowInput.TimeSpeedUpButton, () => Increase(_input.TimeFlowInput.TimeSpeedUpButton.Value));
        _handlers.Add(_input.TimeFlowInput.TimeSlowDownButton, () => Decrease(_input.TimeFlowInput.TimeSlowDownButton.Value));
    }

    public IAmount DeltaTimeFactor => _deltaTimeFactor;

    public void Start()
    {
        foreach (var pair in _handlers)
        {
            pair.Key.Pressed += pair.Value;
        }
    }

    public void Finish()
    {
        foreach (var pair in _handlers)
        {
            pair.Key.Pressed -= pair.Value;
        }
    }

    private void Increase(float increment)
    {
        _deltaTimeFactor.Increase(increment);
    }

    private void Decrease(float decrement)
    {
        _deltaTimeFactor.Decrease(decrement);
    }

    private void Change(float newCoef)
    {
        _deltaTimeFactor.Change(newCoef);
    }
}