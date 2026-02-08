using System;
using System.Collections.Generic;

public class KeyboardInputCreator
{
    private readonly EventBus _eventBus;
    private readonly KeyboardInputSettings _keyboardInputSettings;

    public KeyboardInputCreator(EventBus eventBus, KeyboardInputSettings keyboardInputSettings)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _keyboardInputSettings = keyboardInputSettings ? keyboardInputSettings : throw new ArgumentNullException(nameof(keyboardInputSettings));
    }

    public KeyboardPlayingInput CreatePlayingInput()
    {
        return new KeyboardPlayingInput(new KeyboardInput(_keyboardInputSettings.PauseButton),
                                        new KeyboardContinuousInput(_keyboardInputSettings.InteractButton,
                                                                    _keyboardInputSettings.IsOneClick));
    }

    public KeyboardPauseInput CreatePauseInput()
    {
        return new KeyboardPauseInput(new KeyboardInput(_keyboardInputSettings.PauseButton));
    }

    public KeyboardSwapAbilityInput CreateSwapAbilityInput()
    {
        return new KeyboardSwapAbilityInput(new KeyboardContinuousInput(_keyboardInputSettings.InteractButton,
                                                                        _keyboardInputSettings.IsOneClick));
    }

    public BackgroundInput CreateBackgroundInput()
    {
        List<TimeButton> timeButtons = new List<TimeButton>
        {
            _keyboardInputSettings.VerySlowTimeButton,
            _keyboardInputSettings.SlowTimeButton,
            _keyboardInputSettings.NormalTimeButton,
            _keyboardInputSettings.FastTimeButton,
            _keyboardInputSettings.VeryFastTimeButton
        };

        return new BackgroundInput(_eventBus,
                                   new KeyboardInput(_keyboardInputSettings.ResetSceneButton),
                                   new KeyboardInput(_keyboardInputSettings.SwitchUiButton),
                                   timeButtons,
                                   _keyboardInputSettings.IncreasedTimeButton,
                                   _keyboardInputSettings.DecreasedTimeButton);
    }
}