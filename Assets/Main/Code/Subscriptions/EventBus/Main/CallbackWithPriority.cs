using System;

public class CallbackWithPriority
{
    private readonly object _callback;
    private readonly Priority _priority;
    private readonly bool _isOneTimeSubscription;

    public CallbackWithPriority(object callback, Priority priority, bool isOneTimeSubscription)
    {
        _callback = callback ?? throw new ArgumentNullException(nameof(callback));
        _priority = priority;
        _isOneTimeSubscription = isOneTimeSubscription;
    }

    public object Callback => _callback;

    public Priority Priority => _priority;

    public bool IsOneTimeSubscription => _isOneTimeSubscription;
}