using System;
using System.Collections.Generic;
using System.Linq;

public abstract class Storage<T> where T : IDestroyable
{
    private readonly List<T> _storagables;

    public Storage()
    {
        _storagables = new List<T>();
    }

    public void Clear()
    {
        for (int storagable = _storagables.Count - 1; storagable >= 0; storagable--)
        {
            UnsubscribeFromStoragable(_storagables[storagable]);
        }

        _storagables.Clear();
    }

    public void Add(T storagable)
    {
        if (storagable == null)
        {
            throw new ArgumentNullException(nameof(storagable));
        }

        if (Contains(storagable))
        {
            throw new InvalidOperationException($"{nameof(storagable)} is already added");
        }

        SubscribeToStoragable(storagable);

        _storagables.Add(storagable);
    }

    public void Remove(T storagable)
    {
        if (storagable == null)
        {
            throw new ArgumentNullException(nameof(storagable));
        }

        if (Contains(storagable) == false)
        {
            throw new InvalidOperationException($"{nameof(storagable)} is not contained");
        }

        UnsubscribeFromStoragable(storagable);

        _storagables.Remove(storagable);
    }

    public bool TryGet(object searchParameter, out T storagable)
    {
        storagable = _storagables.FirstOrDefault(element => element.Equals(searchParameter));

        return storagable != null;
    }

    protected virtual void SubscribeToAdditionalEvents(T storagable)
    {

    }

    protected virtual void UnsubscribeFromAdditionalEvents(T storagable)
    {

    }

    protected void UnsubscribeFromDestroyableWithRemoving(IDestroyable destroyable)
    {
        if (destroyable is not T storagable)
        {
            throw new InvalidCastException($"{nameof(destroyable)} is not {typeof(T)}");
        }

        UnsubscribeFromStoragable(storagable);

        _storagables.Remove(storagable);
    }

    private void SubscribeToStoragable(T storagable)
    {
        storagable.DestroyedIDestroyable += UnsubscribeFromDestroyableWithRemoving;

        SubscribeToAdditionalEvents(storagable);
    }

    private void UnsubscribeFromStoragable(T storagable)
    {
        storagable.DestroyedIDestroyable -= UnsubscribeFromDestroyableWithRemoving;

        UnsubscribeFromAdditionalEvents(storagable);
    }

    private bool Contains(T other)
    {
        return _storagables.FirstOrDefault(storagable => storagable.Equals(other)) != null;
    }
}