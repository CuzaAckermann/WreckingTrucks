using System;
using UnityEngine;

public class SpawnDetector : Creatable
{
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private GameObjectTriggerDetector _triggerDetector;

    private GameObjectDetectorWaitingState _waitingState;

    private Transform _transform;

    private int _amountObjectsOfEntered;

    private bool _isActivated = false;

    public override void Init()
    {
        _transform = transform;

        _waitingState = new GameObjectDetectorWaitingState(_triggerDetector);
    }

    public event Action Empty;

    private void OnEnable()
    {
        if (_isActivated == false)
        {
            return;
        }

        StartDetect();
    }

    private void OnDisable()
    {
        _waitingState.Exit();
    }

    public void SetPosition(Vector3 position, Vector3 direction)
    {
        _transform.position = position;
        _transform.forward = direction;
    }

    public void StartDetect()
    {
        if (IsEmpty())
        {
            Empty?.Invoke();
        }
        else
        {
            _waitingState.Enter(OnDetected, OnLeaved);
        }

        _isActivated = true;
    }

    public void FinishDetect()
    {
        _isActivated = false;
        _waitingState.Exit();
    }

    private bool IsEmpty()
    {
        Collider[] colliders = Physics.OverlapBox(_transform.position,
                                                  _boxCollider.bounds.extents,
                                                  _transform.rotation,
                                                  _layerMask);

        _amountObjectsOfEntered = 0;

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] != _boxCollider)
            {
                _amountObjectsOfEntered++;
            }
        }

        return _amountObjectsOfEntered == 0;
    }

    private void OnDetected(GameObject _)
    {
        _amountObjectsOfEntered++;
    }

    private void OnLeaved(GameObject _)
    {
        _amountObjectsOfEntered--;

        if (_amountObjectsOfEntered <= 0)
        {
            Empty?.Invoke();
        }
    }
}