public class KeyboardInputCreator
{
    private readonly KeyboardInputSettings _keyboardInputSettings;

    private readonly KeyboardInput _keyboardInput;
    private readonly DeveloperInput _developerInput;

    public KeyboardInputCreator(KeyboardInputSettings keyboardInputSettings)
    {
        Validator.ValidateNotNull(keyboardInputSettings);

        _keyboardInputSettings = keyboardInputSettings;

        _keyboardInput = CreateKeyboardInput();
        _developerInput = CreateDeveloperInput();
    }

    public KeyboardInput GetKeyboardInput()
    {
        return _keyboardInput;
    }

    public DeveloperInput GetDeveloperInput()
    {
        return _developerInput;
    }

    private KeyboardInput CreateKeyboardInput()
    {
        IPressMode pressMode = GetPressMode();

        return new KeyboardInput(new KeyboardButton(_keyboardInputSettings.PauseButton, pressMode),
                                 new KeyboardButton(_keyboardInputSettings.InteractButton, pressMode));
    }

    private DeveloperInput CreateDeveloperInput()
    {
        IPressMode pressMode = GetPressMode();

        ITimeFlowInput timeFlowInput = new TimeFlowInput(_keyboardInputSettings.TimeFlowSettings.VerySlowTimeButton,
                                                         _keyboardInputSettings.TimeFlowSettings.SlowTimeButton,
                                                         _keyboardInputSettings.TimeFlowSettings.NormalTimeButton,
                                                         _keyboardInputSettings.TimeFlowSettings.FastTimeButton,
                                                         _keyboardInputSettings.TimeFlowSettings.VeryFastTimeButton,
                                                         _keyboardInputSettings.TimeFlowSettings.DecreasedTimeButton,
                                                         _keyboardInputSettings.TimeFlowSettings.IncreasedTimeButton);

        return new DeveloperInput(new KeyboardButton(_keyboardInputSettings.ResetSceneButton, pressMode),
                                  new KeyboardButton(_keyboardInputSettings.SwitchUiButton, pressMode),
                                  timeFlowInput,
                                  new KeyboardButton(_keyboardInputSettings.PauseButton, pressMode),
                                  new KeyboardButton(_keyboardInputSettings.InteractButton, pressMode));
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