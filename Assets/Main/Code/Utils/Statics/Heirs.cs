using System;
using System.Collections.Generic;
using System.Linq;

public static class Heirs
{
    private static readonly Dictionary<Type, Type[]> _parentWithHeirs = new Dictionary<Type, Type[]>();

    public static Type[] Of<T>()
    {
        Type baseType = typeof(T);

        if (_parentWithHeirs.ContainsKey(baseType) == false)
        {
            _parentWithHeirs.Add(baseType, GetHeirs(baseType));
        }

        Logger.Log(_parentWithHeirs[baseType].Length);

        return _parentWithHeirs[baseType];
    }

    private static Type[] GetHeirs(Type baseType)
    {
        return baseType.Assembly.GetTypes().Where(heir => heir.IsClass &&
                                                          heir.IsAbstract == false &&
                                                          baseType.IsAssignableFrom(heir))
                                           .Where(heir => heir != baseType)
                                           .ToArray();
    }
}