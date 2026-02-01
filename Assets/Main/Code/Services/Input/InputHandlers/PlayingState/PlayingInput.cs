using System;

public class PlayingInput
{
    private readonly IElementInput _pauseInput;
    private readonly IElementInput _interactInput;

    public PlayingInput(IElementInput pauseInput, IElementInput interactInput)
    {
        _pauseInput = pauseInput ?? throw new ArgumentNullException(nameof(pauseInput));
        _interactInput = interactInput ?? throw new ArgumentNullException(nameof(interactInput));
    }

    public event Action PausePressed;
    public event Action InteractPressed;

    public void Update()
    {
        if (_pauseInput.IsPressed())
        {
            PausePressed?.Invoke();
        }

        if (_interactInput.IsPressed())
        {
            InteractPressed?.Invoke();
        }
    }
}