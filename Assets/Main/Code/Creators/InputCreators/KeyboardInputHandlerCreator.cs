using System;

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
}