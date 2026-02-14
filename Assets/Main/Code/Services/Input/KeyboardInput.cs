public class KeyboardInput : IInput
{
    public KeyboardInput(KeyboardButton pauseInput, KeyboardButton interactInput)
    {
        Validator.ValidateNotNull(pauseInput, interactInput);

        PauseButton = pauseInput;
        InteractButton = interactInput;
    }

    public IInputButton PauseButton { get; private set; }

    public IInputButton InteractButton { get; private set; }
}