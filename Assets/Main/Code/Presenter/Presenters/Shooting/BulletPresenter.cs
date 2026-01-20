using UnityEngine;

public class BulletPresenter : Presenter
{
    [SerializeField] private GameObjectColliderDetector _gameObjectColliderDetector;
    [SerializeField] private TrailRenderer _tail;

    private PresenterDetector _presenterDetector;

    private bool _isSubscribedBlockPresenterDetector;

    private bool _isInitialized;

    public override void Init()
    {
        base.Init();

        _presenterDetector = new PresenterDetector(_gameObjectColliderDetector);

        _isInitialized = true;
    }

    protected override void Subscribe()
    {
        base.Subscribe();

        _tail.gameObject.SetActive(true);
        _tail.ResetBounds();

        if (_isInitialized)
        {
            if (_isSubscribedBlockPresenterDetector == false)
            {
                _presenterDetector.Detected += OnPresenterDetected;
                _presenterDetector.Enable();

                _isSubscribedBlockPresenterDetector = true;
            }
        }
    }

    protected override void Unsubscribe()
    {
        base.Unsubscribe();

        _tail.gameObject.SetActive(false);

        if (_isInitialized)
        {
            if (_isSubscribedBlockPresenterDetector)
            {
                _presenterDetector.Disable();
                _presenterDetector.Detected -= OnPresenterDetected;

                _isSubscribedBlockPresenterDetector = false;
            }
        }
    }

    private void OnPresenterDetected(Presenter detectablePresenter)
    {
        if (Model is Bullet bullet)
        {
            if (detectablePresenter.Model is Block block)
            {
                bullet.DestroyBlock(block);
            }
        }
    }
}