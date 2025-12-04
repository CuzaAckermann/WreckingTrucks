using System;
using UnityEngine;

public class GameObjectTriggerDetector : MonoBehaviour, IDetector
{
    [SerializeField] private Collider _collider;

    public void Init()
    {
        if (_collider.isTrigger == false)
        {
            throw new ArgumentException($"{nameof(Collider)} is not trigger");
        }
    }

    public event Action<GameObject> Detected;

    public event Action<GameObject> Leaved;

    private void OnTriggerEnter(Collider collider)
    {
        Detected?.Invoke(collider.gameObject);
    }

    private void OnTriggerExit(Collider collider)
    {
        Leaved?.Invoke(collider.gameObject);
    }
}