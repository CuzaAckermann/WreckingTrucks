using System;
using UnityEngine;

public class GameObjectDetectorWaitingState
{
    private readonly GameObjectTriggerDetector _triggerDetector;

    private Action<GameObject> _handlerForDetected;
    private Action<GameObject> _handlerForLeaved;

    private bool _isSubscribed;

    public GameObjectDetectorWaitingState(GameObjectTriggerDetector triggerDetector)
    {
        _triggerDetector = triggerDetector ? triggerDetector : throw new ArgumentNullException(nameof(triggerDetector));
    }

    public void Enter(Action<GameObject> handlerForDetected,
                      Action<GameObject> handlerForLeaved)
    {
        if (_isSubscribed == false)
        {
            _handlerForDetected = handlerForDetected;
            _handlerForLeaved = handlerForLeaved;

            _triggerDetector.Detected += _handlerForDetected;
            _triggerDetector.Leaved += _handlerForLeaved;

            _isSubscribed = true;
        }
        else
        {
            //Logger.Log($"Already subscribed to {GetType()}");
        }
    }

    public void Exit()
    {
        if (_isSubscribed)
        {
            _triggerDetector.Detected -= _handlerForDetected;
            _triggerDetector.Leaved -= _handlerForLeaved;

            _isSubscribed = false;
        }
        else
        {
            //Logger.Log($"Already unsubscribed from {GetType()}");
        }
    }
}