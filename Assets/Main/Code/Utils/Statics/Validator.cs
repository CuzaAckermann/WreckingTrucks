using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
            throw new ArgumentNullException(obj.GetType().Name);
        }
    }

    public static void ValidateNotNull(params object[] objs)
    {
        foreach (object obj in objs)
        {
            ValidateNotNull(obj);
        }
    }

    public static void ValidateMin<T>(T value, T min, bool shouldThrowIfEqual) where T : IComparable<T>
    {
        if (shouldThrowIfEqual == false)
        {
            if (value.CompareTo(min) < 0)
            {
                //throw new ArgumentOutOfRangeException($"{nameof(value)} - {value} must be greater than or equal to {nameof(min)} - {min}");
            }
        }
        else
        {
            if (value.CompareTo(min) <= 0)
            {
                //throw new ArgumentOutOfRangeException($"{nameof(value)} - {value} must be greater than {nameof(min)} - {min}");
            }
        }
    }

    public static void ValidateMax<T>(T value, T max, bool shouldThrowIfEqual) where T : IComparable<T>
    {
        if (shouldThrowIfEqual == false)
        {
            if (value.CompareTo(max) > 0)
            {
                //throw new ArgumentOutOfRangeException($"{nameof(value)} - {value} must be less than or equal to {nameof(max)} - {max}");
            }
        }
        else
        {
            if (value.CompareTo(max) >= 0)
            {
                //throw new ArgumentOutOfRangeException($"{nameof(value)} - {value} must be less than {nameof(max)} - {max}");
            }
        }
    }

    public static bool IsContains<T>(IReadOnlyCollection<T> collection, T item)
    {
        if (collection.Contains(item))
        {
            //Logger.Log($"{nameof(collection)} contain {nameof(item)}");

            return true;
        }
        else
        {
            //Logger.Log($"{nameof(collection)} not contain {nameof(item)}");

            return false;
        }
    }

    public static void ValidateNotEmpty(ICollection collection)
    {
        if (collection.Count == 0)
        {
            //throw new InvalidOperationException($"{nameof(collection)} is empty!");
        }
    }
}