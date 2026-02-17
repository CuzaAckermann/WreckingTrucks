using System;
using System.Collections.Generic;
using UnityEngine;

public static class Logger
{
    private static int _count;

    public static void Reset()
    {
        _count = 0;
    }

    public static void Log()
    {
        Log(++_count);
    }

    public static void Log<T>(T message)
    {
        Debug.Log($"{message}");
    }

    public static void LogError<T>(T message)
    {
        Debug.LogError(message);
    }

    public static void Log<T>(params T[] values)
    {
        string sum = string.Empty;

        for (int i = 0; i < values.Length; i++)
        {
            sum += values[i].ToString();

            if (i != values.Length - 1)
            {
                sum += ", ";
            }
        }

        Log(sum);
    }

    public static void LogProbabilities(Dictionary<Type, float> probabilities)
    {
        Log(new String('=', 30));

        foreach (var probability in probabilities)
        {
            Log($"{probability.Key} - {probability.Value}");
        }

        Log(new String('=', 30));
    }

    public static void Log<T>(Type sender, T message)
    {
        Log($"{sender} - {message}");
    }
}