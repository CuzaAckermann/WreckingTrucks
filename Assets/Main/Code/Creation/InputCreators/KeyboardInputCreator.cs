using System.Collections.Generic;

public class KeyboardInputCreator : IInputCreator<KeyboardInput>
{
    private readonly KeyboardInputSettings _keyboardInputSettings;

    private readonly KeyboardInput _keyboardInput;

    public KeyboardInputCreator(KeyboardInputSettings keyboardInputSettings)
    {
        Validator.ValidateNotNull(keyboardInputSettings);

        _keyboardInputSettings = keyboardInputSettings;

        _keyboardInput = Create();
    }

    public KeyboardInput GetKeyboardInput()
    {
        return _keyboardInput;
    }

    public KeyboardInput Create()
    {
        IPressMode pressMode = GetPressMode();

        List<ValueButton> buttons = new List<ValueButton>();

        for (int currentSettings = 0; currentSettings < _keyboardInputSettings.TimeFlowSettings.TimeFactorButtons.Count; currentSettings++)
        {
            ValueButtonSettings buttonSettings = _keyboardInputSettings.TimeFlowSettings.TimeFactorButtons[currentSettings];

            buttons.Add(new ValueButton(buttonSettings.Button, pressMode, buttonSettings.Value));
        }

        ValueButton timeSlowDownButton = new ValueButton(_keyboardInputSettings.TimeFlowSettings.TimeSlowDownButton.Button,
                                                         pressMode,
                                                         _keyboardInputSettings.TimeFlowSettings.TimeSlowDownButton.Value);

        ValueButton timeSpeedUpButton = new ValueButton(_keyboardInputSettings.TimeFlowSettings.TimeSpeedUpButton.Button,
                                                        pressMode,
                                                        _keyboardInputSettings.TimeFlowSettings.TimeSpeedUpButton.Value);

        ITimeFlowInput timeFlowInput = new TimeFlowInput(buttons,
                                                         timeSlowDownButton,
                                                         timeSpeedUpButton);

        return new KeyboardInput(new KeyboardButton(_keyboardInputSettings.PauseButton, pressMode),
                                 new KeyboardButton(_keyboardInputSettings.InteractButton, pressMode),
                                 new KeyboardButton(_keyboardInputSettings.ResetSceneButton, pressMode),
                                 new KeyboardButton(_keyboardInputSettings.SwitchUiButton, pressMode),
                                 timeFlowInput);
    }

    private IPressMode GetPressMode()
    {
        return _keyboardInputSettings.PressMode switch
        {
            PressModeEnum.Down => new KeyDownMode(),
            PressModeEnum.Up => new KeyUpMode(),
            PressModeEnum.Press => new KeyPressMode(),
            _ => null,
        };
    }
}