using System;
using System.Collections.Generic;

public class Bullet
{
    private readonly HashSet<Type> _typesToDestroy = new HashSet<Type>();

    public void AddTypeToDestroy<T>() where T : Block
    {
        Type type = typeof(T);

        if (_typesToDestroy.Add(type) == false)
        {
            throw new InvalidOperationException($"{type} has already been added.");
        }
    }

    public bool CanDestroyBlock(Block block) => _typesToDestroy.Contains(block.GetType());

    public void Clear()
    {
        _typesToDestroy.Clear();
    }
}