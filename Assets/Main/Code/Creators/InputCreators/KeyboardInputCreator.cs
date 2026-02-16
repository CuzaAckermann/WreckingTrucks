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

        ITimeFlowInput timeFlowInput = new TimeFlowInput(_keyboardInputSettings.TimeFlowSettings.TimeButtons,
                                                         _keyboardInputSettings.TimeFlowSettings.DecreasedTimeButton,
                                                         _keyboardInputSettings.TimeFlowSettings.IncreasedTimeButton);

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