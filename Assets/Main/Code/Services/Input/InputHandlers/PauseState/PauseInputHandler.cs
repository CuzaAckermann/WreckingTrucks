using System;

public class PauseInputHandler
{
    private readonly IPauseInput _pauseInput;

    public PauseInputHandler(IPauseInput pauseInput)
    {
        _pauseInput = pauseInput ?? throw new ArgumentNullException(nameof(pauseInput));
    }

    public event Action PausePressed;

    public void Update()
    {
        if (_pauseInput.IsPressed())
        {
            PausePressed?.Invoke();
        }
    }
}