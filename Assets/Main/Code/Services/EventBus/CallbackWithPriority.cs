using System;

public class CallbackWithPriority
{
    private readonly object _callback;
    private readonly int _priority;

    public CallbackWithPriority(object callback, int priority)
    {
        _callback = callback ?? throw new ArgumentNullException(nameof(callback));
        _priority = priority >= 0 ? priority : throw new ArgumentOutOfRangeException(nameof(priority));
    }

    public object Callback => _callback;

    public int Priority => _priority;
}