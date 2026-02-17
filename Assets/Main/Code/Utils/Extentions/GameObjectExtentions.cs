using UnityEngine;

public static class GameObjectExtentions
{
    public static void On(this GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    public static void Off(this GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}