using System;

public class PlayingInputHandler
{
    private readonly IPauseInput _pauseInput;
    private readonly IInteractInput _interactInput;

    public PlayingInputHandler(IPauseInput pauseInput, IInteractInput interactInput)
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