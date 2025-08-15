using System;
using UnityEngine;

public abstract class CollisionPresenterDetector<T> : MonoBehaviour where T : Presenter
{
    public event Action<T> Detected;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out T presenter))
        {
            Detected?.Invoke(presenter);
        }
    }
}