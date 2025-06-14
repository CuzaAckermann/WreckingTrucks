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
}