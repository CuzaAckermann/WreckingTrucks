using System;
using System.Collections.Generic;
using UnityEngine;

public static class Logger
{
    public static int _count;

    public static void Log()
    {
        Debug.Log(++_count);
    }

    public static void Log<T>(T message)
    {
        Debug.Log($"{message}");
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
}