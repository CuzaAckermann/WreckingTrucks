using System;

public class Command
{
    private readonly Action _action;
    private readonly float _delay;

    public Command(Action action, float delay)
    {
        _action = action ?? throw new ArgumentNullException(nameof(action));
        _delay = delay >= 0 ? delay : throw new ArgumentOutOfRangeException(nameof(delay));
    }

    public event Action<Command> Canceled;

    public event Action<Command> Executed;

    public float Delay => _delay;

    public void Cancel()
    {
        Canceled?.Invoke(this);
    }

    public void Execute()
    {
        _action.Invoke();

        Executed?.Invoke(this);
    }
}