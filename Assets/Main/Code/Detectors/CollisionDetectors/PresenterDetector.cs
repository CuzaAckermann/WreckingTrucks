using System;
using UnityEngine;

public class PresenterDetector
{
    private readonly GameObjectColliderDetector _gameObjectColliderDetector;

    private bool _isSubscribed;

    public PresenterDetector(GameObjectColliderDetector gameObjectColliderDetector)
    {
        _gameObjectColliderDetector = gameObjectColliderDetector ? gameObjectColliderDetector : throw new ArgumentNullException(nameof(gameObjectColliderDetector));

        _isSubscribed = false;
    }

    public event Action<Presenter> Detected;

    public void Enable()
    {
        if (_isSubscribed == false)
        {
            _gameObjectColliderDetector.Detected += OnDetected;

            _isSubscribed = true;
        }
    }

    public void Disable()
    {
        if (_isSubscribed)
        {
            _gameObjectColliderDetector.Detected -= OnDetected;

            _isSubscribed = false;
        }
    }

    private void OnDetected(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out Presenter presenter))
        {
            Detected?.Invoke(presenter);
        }
    }
}