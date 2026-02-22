using System;
using System.Collections.Generic;

public class Storage<S> : IStorage<S>
{
    private readonly HashSet<S> _storagables = new HashSet<S>();

    public Storage(List<S> storagables)
    {
        Validator.ValidateNotNull(storagables);

        foreach (S storagable in storagables)
        {
            Add(storagable);
        }
    }

    public void Clear()
    {
        _storagables.Clear();
    }

    public void Add(S uniqueItem)
    {
        Validator.ValidateNotNull(uniqueItem);

        if (Validator.IsContains(_storagables, uniqueItem))
        {
            return;
        }

        _storagables.Add(uniqueItem);
    }

    public void Remove(S removedItem)
    {
        Validator.ValidateNotNull(removedItem);

        if (Validator.IsContains(_storagables, removedItem) == false)
        {
            return;
        }

        _storagables.Remove(removedItem);
    }

    public List<S> GetAll()
    {
        return new List<S>(_storagables);
    }

    public bool TryGet(Type type, out S foundItem)
    {
        foundItem = default;

        foreach (S item in _storagables)
        {
            if (item.GetType() == type)
            {
                foundItem = item;

                return true;
            }
        }

        return false;
    }

    public bool TryGet<T>(out T foundItem) where T : S
    {
        foundItem = default;

        if (TryGet(typeof(T), out S item))
        {
            foundItem = (T)item;

            return true;
        }

        return false;
    }

    public T Get<T>() where T : S
    {
        foreach (S item in _storagables)
        {
            if (item is T foundItem)
            {
                return foundItem;
            }
        }

        return default;
    }
}