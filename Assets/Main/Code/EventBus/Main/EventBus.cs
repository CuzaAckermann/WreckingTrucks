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

    public void Subscribe<T>(Action<T> callback, Priority priority = Priority.Medium, bool isOneTimeSubscription = false) where T : EventBusSignal
    {
        string key = typeof(T).Name;

        if (_signalCallbacks.ContainsKey(key) == false)
        {
            _signalCallbacks.Add(key, new List<CallbackWithPriority> { new CallbackWithPriority(callback, priority, isOneTimeSubscription) });

            return;
        }

        CallbackWithPriority existingCallback = _signalCallbacks[key].FirstOrDefault(callbackWithPriority => callbackWithPriority.Callback.Equals(callback));

        if (existingCallback != null)
        {
            //Logger.Log($"{nameof(callback)} is already subscribed to {key}");

            return;
        }

        _signalCallbacks[key].Add(new CallbackWithPriority(callback, priority, isOneTimeSubscription));

        _signalCallbacks[key] = _signalCallbacks[key].OrderBy(callbackWithPriority => callbackWithPriority.Priority).ToList();
    }

    public void Unsubscribe<T>(Action<T> callback) where T : EventBusSignal
    {
        string key = typeof(T).Name;

        if (_signalCallbacks.ContainsKey(key) == false)
        {
            Logger.Log($"{key} does not exist");

            return;
        }

        CallbackWithPriority callbackToDelete = _signalCallbacks[key].FirstOrDefault(callbackWithPriority => callbackWithPriority.Callback.Equals(callback));

        if (callbackToDelete == null)
        {
            //Logger.Log($"{nameof(callback)} is not subscribed to {key}");

            return;
        }

        _signalCallbacks[key].Remove(callbackToDelete);
    }

    public void Invoke<T>(T signal) where T : EventBusSignal
    {
        string key = typeof(T).Name;

        if (_signalCallbacks.ContainsKey(key) == false)
        {
            //Logger.Log($"No subscribers for {key}");

            return;
        }

        for (int i = _signalCallbacks[key].Count - 1; i >= 0; i--)
        {
            CallbackWithPriority callbackWithPriority = _signalCallbacks[key][i];

            if (callbackWithPriority.Callback is not Action<T> callback)
            {
                continue;
            }

            callback?.Invoke(signal);

            //if (callbackWithPriority.IsOneTimeSubscription == false)
            //{
            //    return;
            //}

            //_signalCallbacks[key].Remove(callbackWithPriority);
        }
    }
}