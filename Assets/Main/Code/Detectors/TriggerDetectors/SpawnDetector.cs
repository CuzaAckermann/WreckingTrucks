using System;
using UnityEngine;

public class SpawnDetector : Creatable
{
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private GameObjectTriggerDetector _triggerDetector;

    private Transform _transform;

    private int _amountObjectsOfEntered;

    private bool _isSubscribed = false;

    public override void Init()
    {
        _transform = transform;

        DefineAmountColliders();
    }

    public event Action Empty;

    private void OnEnable()
    {
        SubscribeToDetector();
    }

    private void OnDisable()
    {
        UnsubscribeFromDetector();
    }

    public bool IsEmpty(Vector3 position, Vector3 direction)
    {
        _transform.position = position;
        _transform.forward = direction;

        return _amountObjectsOfEntered == 0;
    }

    private void DefineAmountColliders()
    {
        Collider[] colliders = Physics.OverlapBox(_boxCollider.bounds.center,
                                                  _boxCollider.bounds.extents,
                                                  _transform.rotation,
                                                  _layerMask);

        _amountObjectsOfEntered = colliders.Length;
    }

    private void SubscribeToDetector()
    {
        if (_isSubscribed == false)
        {
            _triggerDetector.Detected += OnDetected;
            _triggerDetector.Leaved += OnLeaved;

            _isSubscribed = true;
        }
    }

    private void UnsubscribeFromDetector()
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
        _amountObjectsOfEntered++;
    }

    private void OnLeaved(GameObject gameObject)
    {
        _amountObjectsOfEntered--;

        if (_amountObjectsOfEntered <= 0)
        {
            Empty?.Invoke();
        }
    }
}