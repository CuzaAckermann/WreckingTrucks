using System;
using System.Collections.Generic;

public class AutoRemovingStorage<D> where D : IDestroyable
{
    private readonly HashSet<D> _destroyables;

    public AutoRemovingStorage(int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(capacity)} must be positive");
        }

        _destroyables = new HashSet<D>(capacity);
    }

    public void Clear()
    {
        foreach (D destroyable in _destroyables)
        {
            UnsubscribeFromCreated(destroyable);
        }

        _destroyables.Clear();
    }

    public void ClearWithDestroy()
    {
        foreach (D destroyable in _destroyables)
        {
            UnsubscribeFromCreated(destroyable);

            destroyable.Destroy();
        }

        _destroyables.Clear();
    }

    public void Add(D item)
    {
        if (Validator.IsContains(_destroyables, item))
        {
            return;
        }

        _destroyables.Add(item);

        SubscribeToCreated(item);
    }
    private void SubscribeToCreated(D item)
    {
        item.Destroyed += OnDestroyed;
    }

    private void UnsubscribeFromCreated(IDestroyable item)
    {
        item.Destroyed -= OnDestroyed;
    }

    private void OnDestroyed(IDestroyable item)
    {
        UnsubscribeFromCreated(item);

        if (Validator.IsRequiredType(item, out D required) == false)
        {
            return;
        }

        if (Validator.IsContains(_destroyables, required) == false)
        {
            return;
        }

        _destroyables.Remove(required);
    }
}
