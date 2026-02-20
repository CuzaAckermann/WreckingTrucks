using System;
using System.Collections.Generic;

public class Storage<S>
{
    private readonly HashSet<S> _storagables = new HashSet<S>();

    public virtual void Clear()
    {
        _storagables.Clear();
    }

    public virtual void Add(S uniqueItem)
    {
        Validator.ValidateNotNull(uniqueItem);

        if (Validator.IsContains(_storagables, uniqueItem))
        {
            return;
        }

        _storagables.Add(uniqueItem);
    }

    public virtual void Remove(S removedItem)
    {
        Validator.ValidateNotNull(removedItem);

        if (Validator.IsContains(_storagables, removedItem) == false)
        {
            return;
        }

        _storagables.Remove(removedItem);
    }

    public bool TryFind(Type type, out S foundItem)
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

    public bool TryFind<T>(out T foundItem) where T : S
    {
        foundItem = default;

        if (TryFind(typeof(T), out S item))
        {
            foundItem = (T)item;

            return true;
        }

        return false;
    }
}