using UnityEngine;

public static class Logger
{
    public static int _count;

    public static void Log()
    {
        Debug.Log(++_count);
    }
}