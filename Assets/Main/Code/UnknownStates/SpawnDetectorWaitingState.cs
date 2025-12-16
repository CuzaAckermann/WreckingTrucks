using System;
using UnityEngine;

public class SpawnDetectorWaitingState
{
    private readonly SpawnDetector _spawnDetector;

    private Action _handlerForEmpty;

    private bool _isSubscribed;

    public SpawnDetectorWaitingState(SpawnDetector spawnDetector)
    {
        _spawnDetector = spawnDetector ? spawnDetector : throw new ArgumentNullException(nameof(spawnDetector));
    }

    public void Enter(Vector3 position, Vector3 direction, Action handlerForEmpty)
    {
        if (_isSubscribed == false)
        {
            _handlerForEmpty = handlerForEmpty ?? throw new ArgumentNullException(nameof(handlerForEmpty));

            _spawnDetector.Empty += _handlerForEmpty;
            _isSubscribed = true;

            _spawnDetector.SetPosition(position, direction);
            _spawnDetector.StartDetect();
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
            _spawnDetector.FinishDetect();

            _spawnDetector.Empty -= _handlerForEmpty;
            _isSubscribed = false;
        }
        else
        {
            Logger.Log($"Already unsubscribed from {GetType()}");
        }
    }
}