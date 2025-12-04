using System;
using UnityEngine;

public class GameObjectColliderDetector : MonoBehaviour, IDetector
{
    public event Action<GameObject> Detected;

    public event Action<GameObject> Leaved;

    private void OnCollisionEnter(Collision collision)
    {
        Detected?.Invoke(collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        Leaved?.Invoke(collision.gameObject);
    }
}