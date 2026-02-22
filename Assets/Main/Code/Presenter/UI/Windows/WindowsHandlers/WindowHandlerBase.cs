using System;

public abstract class WindowHandlerBase
{
    private readonly InputStateStorage _storage;

    public WindowHandlerBase(InputStateStorage storage)
    {
        Validator.ValidateNotNull(storage);

        _storage = storage;
    }

    public event Action<InputState> InputStateStarting;

    public event Action InputStateReturning;

    public abstract void Start();

    public abstract void Finish();

    protected void OnInputStateStarting<T>() where T : InputState
    {
        if (_storage.TryGet(out T inputState) == false)
        {
            throw new InvalidOperationException();
        }

        InputStateStarting?.Invoke(inputState);
    }

    protected void OnInputStateReturning()
    {
        InputStateReturning?.Invoke();
    }
}