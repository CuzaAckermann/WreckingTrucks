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

        Debug.Log(sum);
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

    public static void Log(Dictionary<Type, Queue<Block>> blocksByType)
    {
        foreach (var blockByType in blocksByType)
        {
            Log($"{blockByType.Key} - {blockByType.Value.Count}");
        }
    }

    public static void Log(Dictionary<Type, int> amountByType)
    {
        foreach (var amount in amountByType)
        {
            Log($"{amount.Key} - {amount.Value}");
        }
    }

    public static void Log(List<Model> models)
    {
        Dictionary<Type, int> amountByType = new Dictionary<Type, int>();

        for (int i = 0; i < models.Count; i++)
        {
            if (amountByType.ContainsKey(models[i].GetType()) == false)
            {
                amountByType[models[i].GetType()] = 0;
            }

            amountByType[models[i].GetType()]++;
        }

        Log(amountByType);
    }

    public static void LogDelegate(Delegate[] delegates)
    {
        if (delegates == null)
        {
            Log("Тут null");
            return;
        }

        if (delegates.Length == 0)
        {
            Log("Пусто");
        }

        for (int i = 0; i < delegates.Length; i++)
        {
            Log(delegates[i].Method.DeclaringType);
        }
    }
}