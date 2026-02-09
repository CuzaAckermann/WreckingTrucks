using System;
using System.Collections.Generic;

public static class Validator
{
    public static bool IsRequiredType<T>(object obj, out T requiredType)
    {
        ValidateNotNull(obj);

        requiredType = default;

        if (obj is not T type)
        {
            Logger.Log($"{nameof(obj)} is not {nameof(T)}");
            Logger.Log($"{nameof(obj)} is {obj.GetType().Name}");

            return false;
        }

        requiredType = type;

        return true;
    }

    public static void ValidateNotNull(object obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }
    }

    public static bool IsContains<T>(ICollection<T> collection, T item)
    {
        if (collection.Contains(item))
        {
            Logger.Log($"{nameof(collection)} contain {nameof(item)}");

            return true;
        }
        else
        {
            Logger.Log($"{nameof(collection)} not contain {nameof(item)}");

            return false;
        }
    }
}