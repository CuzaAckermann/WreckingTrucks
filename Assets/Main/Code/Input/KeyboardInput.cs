public class KeyboardInput : IInput
{
    public KeyboardInput(KeyboardButton pauseInput,
                         KeyboardButton interactInput,
                         KeyboardButton resetSceneButton,
                         KeyboardButton switchUiButton,
                         ITimeFlowInput timeFlowInput)
    {
        Validator.ValidateNotNull(pauseInput,
                                  interactInput,
                                  resetSceneButton,
                                  switchUiButton,
                                  timeFlowInput);

        PauseButton = pauseInput;
        InteractButton = interactInput;

        ResetSceneButton = resetSceneButton;
        SwitchUiButton = switchUiButton;
        TimeFlowInput = timeFlowInput;
    }

    public IInputButton PauseButton { get; private set; }

    public IInputButton InteractButton { get; private set; }

    public IInputButton ResetSceneButton { get; private set; }

    public IInputButton SwitchUiButton { get; private set; }

    public ITimeFlowInput TimeFlowInput { get; private set; }
}