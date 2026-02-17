using System.Collections.Generic;
using System.Linq;

public class DestroyableStorage<T> where T : IDestroyable
{
    private readonly HashSet<T> _storagables = new HashSet<T>();

    public void Clear()
    {
        foreach (T storagable in _storagables)
        {
            storagable.Destroyed -= UnsubscribeFromStoragable;
        }

        _storagables.Clear();
    }

    public void Add(T storagable)
    {
        Validator.ValidateNotNull(storagable);
        
        if (Validator.IsContains(_storagables, storagable))
        {
            return;
        }

        _storagables.Add(storagable);

        SubscribeToStoragable(storagable);
    }

    public void Remove(T storagable)
    {
        Validator.ValidateNotNull(storagable);

        if (Validator.IsContains(_storagables, storagable) == false)
        {
            return;
        }

        UnsubscribeFromStoragable(storagable);
    }

    public bool TryGet(object searched, out T storagable)
    {
        storagable = _storagables.FirstOrDefault(element => element.Equals(searched));

        return storagable != null;
    }

    private void SubscribeToStoragable(T storagable)
    {
        storagable.Destroyed += UnsubscribeFromStoragable;
    }

    private void UnsubscribeFromStoragable(IDestroyable destroyable)
    {
        if (Validator.IsRequiredType(destroyable, out T storagable) == false)
        {
            return;
        }

        storagable.Destroyed -= UnsubscribeFromStoragable;

        _storagables.Remove(storagable);
    }
}