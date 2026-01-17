using System;
using System.Collections.Generic;
using System.Linq;

public class EventBus
{
    private readonly Dictionary<string, List<CallbackWithPriority>> _signalCallbacks;

    public EventBus()
    {
        _signalCallbacks = new Dictionary<string, List<CallbackWithPriority>>();
    }

    public void Subscribe<T>(Action<T> callback, int priority = 0)
    {
        string key = typeof(T).Name;

        if (_signalCallbacks.ContainsKey(key))
        {
            CallbackWithPriority existingCallback = _signalCallbacks[key].FirstOrDefault(callbackWithPriority => callbackWithPriority.Callback.Equals(callback));

            if (existingCallback == null)
            {
                _signalCallbacks[key].Add(new CallbackWithPriority(callback, priority));

            }
            else
            {
                Logger.LogError("Callback is already subscribed to event");
            }
        }
        else
        {
            _signalCallbacks.Add(key, new List<CallbackWithPriority> { new CallbackWithPriority(callback, priority) });
        }

        _signalCallbacks[key] = _signalCallbacks[key].OrderByDescending(callbackWithPriority => callbackWithPriority.Priority).ToList();
    }

    public void Unsubscribe<T>(Action<T> callback)
    {
        string key = typeof(T).Name;

        if (_signalCallbacks.ContainsKey(key))
        {
            CallbackWithPriority callbackToDelete = _signalCallbacks[key].FirstOrDefault(callbackWithPriority => callbackWithPriority.Callback.Equals(callback));

            if (callbackToDelete != null)
            {
                _signalCallbacks[key].Remove(callbackToDelete);
            }
            else
            {
                Logger.LogError("Callback does not exist");
            }
        }
        else
        {
            Logger.LogError("Event does not exist");
        }
    }

    public void Invoke<T>(T signal)
    {
        string key = typeof(T).Name;

        if (_signalCallbacks.ContainsKey(key))
        {
            foreach (var obj in _signalCallbacks[key])
            {
                Action<T> callback = obj.Callback as Action<T>;

                callback?.Invoke(signal);
            }
        }
        else
        {
            Logger.LogError("Event does not exist");
        }
    }
}