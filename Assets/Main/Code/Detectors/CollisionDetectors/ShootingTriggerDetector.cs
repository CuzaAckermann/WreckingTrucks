using System;
using UnityEngine;

public class ShootingTriggerDetector : MonoBehaviour
{
    public event Action Detected;
    public event Action Leaved;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<ShootingTrigger>() != null)
        {
            Detected?.Invoke();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.GetComponent<ShootingTrigger>() != null)
        {
            Leaved?.Invoke();
        }
    }
}