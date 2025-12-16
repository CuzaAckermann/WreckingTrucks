using System;
using UnityEngine;

public class SpaceDetectorWaitingState<MB> where MB : MonoBehaviour
{
    private readonly TriggerDetector<MB> _triggerDetector;

    private Action<MB> _handlerForDetected;
    private Action _handlerForLeaved;

    private bool _isSubscribed;

    public SpaceDetectorWaitingState(TriggerDetector<MB> triggerDetector)
    {
        _triggerDetector = triggerDetector ?? throw new ArgumentNullException(nameof(triggerDetector));
    }

    public void Enter(Action<MB> handlerForDetected,
                      Action handlerForLeaved)
    {
        if (_isSubscribed == false)
        {
            _handlerForDetected = handlerForDetected;
            _handlerForLeaved = handlerForLeaved;

            _triggerDetector.Detected += _handlerForDetected;
            _triggerDetector.Leaved += _handlerForLeaved;

            _triggerDetector.Enable();

            _isSubscribed = true;
        }
        else
        {
            Logger.Log($"Already subscribed to {GetType()}");
        }
    }

    public void Exit()
    {
        if (_isSubscribed)
        {
            _triggerDetector.Disable();

            _triggerDetector.Detected -= _handlerForDetected;
            _triggerDetector.Leaved -= _handlerForLeaved;

            _isSubscribed = false;
        }
        else
        {
            Logger.Log($"Already unsubscribed from {GetType()}");
        }
    }
}