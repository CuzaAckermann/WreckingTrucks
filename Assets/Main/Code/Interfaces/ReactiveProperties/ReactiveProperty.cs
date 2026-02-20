using System;
using System.Collections.Generic;

public class ReactiveProperty<T>
{
    private readonly List<Delegate> _changedHandlers = new List<Delegate>();
    private readonly List<Delegate> _changedDetailedHandlers = new List<Delegate>();

    private event Action<T> _changed;
    private event Action<T, T> _changedDetailed;

    private readonly IEqualityComparer<T> _comparer;

    public ReactiveProperty(T initialValue = default)
    {
        Value = initialValue;

        _comparer = EqualityComparer<T>.Default;
    }

    public ReactiveProperty(T initialValue, IEqualityComparer<T> comparer)
    {
        Value = initialValue;

        _comparer = comparer ?? EqualityComparer<T>.Default;
    }

    public event Action<T> Changed
    {
        add
        {
            if (_changedHandlers.Contains(value))
            {
                return;
            }

            _changed += value;
            _changedHandlers.Add(value);
        }
        remove
        {
            if (_changedHandlers.Contains(value) == false)
            {
                return;
            }

            _changed -= value;
            _changedHandlers.Remove(value);
        }
    }

    public event Action<T, T> ChangedDetailed
    {
        add
        {
            if (_changedDetailedHandlers.Contains(value))
            {
                return;
            }

            _changedDetailed += value;
            _changedDetailedHandlers.Add(value);
        }
        remove
        {
            if (_changedDetailedHandlers.Contains(value) == false)
            {
                return;
            }

            _changedDetailed -= value;
            _changedDetailedHandlers.Remove(value);
        }
    }

    public T Value { get; private set; }

    public void Dispose()
    {
        _changed = null;
        _changedDetailed = null;

        _changedHandlers.Clear();
        _changedDetailedHandlers.Clear();
    }

    public void SetValue(T newValue)
    {
        if (AreEqual(Value, newValue))
        {
            return;
        }

        T previous = Value;
        Value = newValue;

        _changed?.Invoke(Value);
        _changedDetailed?.Invoke(previous, Value);
    }

    private bool AreEqual(T x, T y)
    {
        if (x == null && y == null)
        {
            return true;
        }

        if (x == null || y == null)
        {
            return false;
        }

        return _comparer.Equals(x, y);
    }
}