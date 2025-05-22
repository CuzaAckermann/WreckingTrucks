using UnityEngine;

public static class Logger
{
    public static int _count;

    public static void Log(string message = "Прок")
    {
        Debug.Log(++_count);
    }
}