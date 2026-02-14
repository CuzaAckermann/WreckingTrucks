public class DeveloperInput : IDeveloperInput
{
    public DeveloperInput(IInputButton resetSceneButton,
                          IInputButton switchUiButton,
                          ITimeFlowInput timeFlowInput,
                          IInputButton pauseButton,
                          IInputButton interactButton)
    {
        Validator.ValidateNotNull(resetSceneButton,
                                  switchUiButton,
                                  timeFlowInput,
                                  pauseButton,
                                  interactButton);

        ResetSceneButton = resetSceneButton;
        SwitchUiButton = switchUiButton;
        TimeFlowInput = timeFlowInput;
        PauseButton = pauseButton;
        InteractButton = interactButton;
    }

    public IInputButton ResetSceneButton { get; private set; }

    public IInputButton SwitchUiButton { get; private set; }

    public ITimeFlowInput TimeFlowInput { get; private set; }

    public IInputButton PauseButton { get; private set; }

    public IInputButton InteractButton { get; private set; }
}