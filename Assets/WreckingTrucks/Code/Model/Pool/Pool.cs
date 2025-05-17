using System;
using System.Collections.Generic;

public class Pool<T> where T : class
{
    private readonly Stack<T> _pool;

    private readonly Func<T> _createFunc;
    private readonly Action<T> _actionOnGet;
    private readonly Action<T> _actionOnRelease;
    private readonly Action<T> _actionOnDestroy;
    private readonly int _maxSize;

    public Pool(Func<T> createFunc,
                Action<T> actionOnGet = null,
                Action<T> actionOnRelease = null,
                Action<T> actionOnDestroy = null,
                int defaultCapacity = 10,
                int maxSize = 10000)
    {
        if (defaultCapacity < 0)
        {
            throw new ArgumentOutOfRangeException($"{defaultCapacity} cannot be negative", nameof(defaultCapacity));
        }

        if (defaultCapacity > maxSize)
        {
            throw new ArgumentException($"{nameof(defaultCapacity)} cannot exceed {nameof(maxSize)}");
        }

        _pool = new Stack<T>(defaultCapacity);
        _createFunc = createFunc ?? throw new ArgumentNullException(nameof(createFunc));
        _actionOnGet = actionOnGet;
        _actionOnRelease = actionOnRelease;
        _actionOnDestroy = actionOnDestroy;
        _maxSize = maxSize > 0 ? maxSize : throw new ArgumentOutOfRangeException($"{maxSize} must be positive", nameof(maxSize));

        Prewarm(defaultCapacity);
    }

    public T GetElement()
    {
        T element = _pool.Count == 0 ? _createFunc() : _pool.Pop();

        if (element == null)
        {
            throw new InvalidOperationException($"{nameof(_createFunc)} returned null");
        }

        _actionOnGet?.Invoke(element);

        return element;
    }

    public void Release(T element)
    {
        if (element == null)
        {
            throw new ArgumentNullException(nameof(element));
        }

        if (_pool.Contains(element))
        {
            throw new InvalidOperationException($"{nameof(element)} already in pool");
        }

        _actionOnRelease?.Invoke(element);

        if (_pool.Count < _maxSize)
        {
            _pool.Push(element);
        }
        else
        {
            _actionOnDestroy?.Invoke(element);
        }
    }

    public void Clear()
    {
        if (_actionOnDestroy != null)
        {
            foreach (T element in _pool)
            {
                _actionOnDestroy(element);
            }
        }

        _pool.Clear();
    }

    private void Prewarm(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Release(_createFunc());
        }
    }
}