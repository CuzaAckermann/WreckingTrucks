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

    public bool TryFind<T>(out T foundItem) where T : class
    {
        foundItem = null;

        foreach (S item in _storagables)
        {
            if (item is T castedItem)
            {
                foundItem = castedItem;

                break;
            }
        }

        return foundItem != null;
    }
}