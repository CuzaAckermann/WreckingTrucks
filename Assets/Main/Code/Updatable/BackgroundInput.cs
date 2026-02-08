using System;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundInput
{
    private readonly EventBus _eventBus;

    private readonly IElementInput _reloadSceneInput;
    private readonly IElementInput _uiSwitchInput;

    private readonly List<TimeButton> _timeButtons;
    private readonly TimeButton _increasedTimeButton;
    private readonly TimeButton _decreasedTimeButton;

    public BackgroundInput(EventBus eventBus,
                           IElementInput reloadSceneInput,
                           IElementInput uiSwitchInput,
                           List<TimeButton> timeButtons,
                           TimeButton increasedTimeButton,
                           TimeButton decreasedTimeButton)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        _reloadSceneInput = reloadSceneInput ?? throw new ArgumentNullException(nameof(reloadSceneInput));
        _uiSwitchInput = uiSwitchInput ?? throw new ArgumentNullException(nameof(uiSwitchInput));

        _timeButtons = timeButtons ?? throw new ArgumentNullException(nameof(timeButtons));
        _increasedTimeButton = increasedTimeButton ?? throw new ArgumentNullException(nameof(increasedTimeButton));
        _decreasedTimeButton = decreasedTimeButton ?? throw new ArgumentNullException(nameof(decreasedTimeButton));

        _eventBus.Subscribe<ClearedSignal<GameSignalEmitter>>(Clear);

        _eventBus.Subscribe<UpdateSignal>(Update);
    }

    public event Action ReloadScenePressed;
    public event Action UiSwitchPressed;

    public event Action<float> TimeCoefficientInstalled;
    public event Action<float> TimeCoefficientIncreased;
    public event Action<float> TimeCoefficientDecreased;

    private void Clear(ClearedSignal<GameSignalEmitter> _)
    {
        _eventBus.Unsubscribe<ClearedSignal<GameSignalEmitter>>(Clear);

        _eventBus.Unsubscribe<UpdateSignal>(Update);
    }

    private void Update(UpdateSignal _)
    {
        if (_reloadSceneInput.IsPressed())
        {
            ReloadScenePressed?.Invoke();
        }

        if (_uiSwitchInput.IsPressed())
        {
            UiSwitchPressed?.Invoke();
        }

        for (int i = 0; i < _timeButtons.Count; i++)
        {
            if (Input.GetKeyDown(_timeButtons[i].Button))
            {
                TimeCoefficientInstalled?.Invoke(_timeButtons[i].TimeCoefficient);

                break;
            }
        }

        if (Input.GetKey(_increasedTimeButton.Button))
        {
            TimeCoefficientIncreased?.Invoke(_increasedTimeButton.TimeCoefficient);
        }

        if (Input.GetKey(_decreasedTimeButton.Button))
        {
            TimeCoefficientDecreased?.Invoke(_decreasedTimeButton.TimeCoefficient);
        }
    }
}