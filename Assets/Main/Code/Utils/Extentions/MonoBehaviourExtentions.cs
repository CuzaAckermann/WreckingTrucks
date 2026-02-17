using UnityEngine;

public static class MonoBehaviourExtentions
{
    public static void On(this MonoBehaviour monoBehaviour)
    {
        monoBehaviour.gameObject.SetActive(true);
    }

    public static void Off(this MonoBehaviour monoBehaviour)
    {
        monoBehaviour.gameObject.SetActive(false);
    }
}