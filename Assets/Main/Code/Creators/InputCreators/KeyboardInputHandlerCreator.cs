using System;
using System.Collections.Generic;

public class KeyboardInputHandlerCreator
{
    private readonly KeyboardInputSettings _keyboardInputSettings;

    public KeyboardInputHandlerCreator(KeyboardInputSettings keyboardInputSettings)
    {
        _keyboardInputSettings = keyboardInputSettings ? keyboardInputSettings : throw new ArgumentNullException(nameof(keyboardInputSettings));
    }

    public KeyboardPlayingInputHandler CreatePlayingInputHandler()
    {
        return new KeyboardPlayingInputHandler(new KeyboardPauseInput(_keyboardInputSettings.PauseButton),
                                               new KeyboardInteractInput(_keyboardInputSettings.InteractButton,
                                                                         _keyboardInputSettings.IsOneClick));
    }

    public KeyboardPauseInputHandler CreatePauseInputHandler()
    {
        return new KeyboardPauseInputHandler(new KeyboardPauseInput(_keyboardInputSettings.PauseButton));
    }

    public KeyboardSwapAbilityInputHandler CreateSwapAbilityInputHandler()
    {
        return new KeyboardSwapAbilityInputHandler(new KeyboardInteractInput(_keyboardInputSettings.InteractButton,
                                                                             _keyboardInputSettings.IsOneClick));
    }

    public SceneReseter CreateSceneReseter()
    {
        return new SceneReseter(_keyboardInputSettings.ResetSceneButton);
    }

    public DeltaTimeCoefficientDefiner CreateDeltaTimeCoefficientDefiner()
    {
        return new DeltaTimeCoefficientDefiner(new List<TimeButton>
                                                   {
                                                       _keyboardInputSettings.VerySlowTimeButton,
                                                       _keyboardInputSettings.SlowTimeButton,
                                                       _keyboardInputSettings.NormalTimeButton,
                                                       _keyboardInputSettings.FastTimeButton,
                                                       _keyboardInputSettings.VeryFastTimeButton
                                                   },
                                               _keyboardInputSettings.DecreasedTimeButton,
                                               _keyboardInputSettings.IncreasedTimeButton);
    }
}