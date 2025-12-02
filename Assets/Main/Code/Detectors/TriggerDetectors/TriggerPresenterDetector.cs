using System;
using UnityEngine;

public abstract class TriggerPresenterDetector<T> : MonoBehaviour where T : Presenter
{
    public event Action<T> Detected;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out T presenter))
        {
            Detected?.Invoke(presenter);
        }
    }
}