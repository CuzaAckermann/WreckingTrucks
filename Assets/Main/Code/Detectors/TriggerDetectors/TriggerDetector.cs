using System;
using UnityEngine;

public class TriggerDetector<MB> where MB : MonoBehaviour
{
    private readonly GameObjectTriggerDetector _triggerDetector;

    private bool _isSubscribed = false;

    public TriggerDetector(GameObjectTriggerDetector triggerDetector)
    {
        triggerDetector.Init();
        _triggerDetector = triggerDetector ? triggerDetector : throw new ArgumentNullException(nameof(triggerDetector));
    }

    public event Action<MB> Detected;

    public event Action Leaved;

    public void Enable()
    {
        if (_isSubscribed == false)
        {
            _triggerDetector.Detected += OnDetected;
            _triggerDetector.Leaved += OnLeaved;

            _isSubscribed = true;
        }
    }

    public void Disable()
    {
        if (_isSubscribed)
        {
            _triggerDetector.Detected -= OnDetected;
            _triggerDetector.Leaved -= OnLeaved;

            _isSubscribed = false;
        }
    }

    private void OnDetected(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out MB monoBehaviour))
        {
            Detected?.Invoke(monoBehaviour);
        }
    }

    private void OnLeaved(GameObject gameObject)
    {
        if (gameObject.GetComponent<MB>() != null)
        {
            Leaved?.Invoke();
        }
    }
}