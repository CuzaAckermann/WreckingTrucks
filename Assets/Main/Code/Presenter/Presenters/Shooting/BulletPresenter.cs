using UnityEngine;

public class BulletPresenter : Presenter
{
    [SerializeField] private GameObjectColliderDetector _gameObjectColliderDetector;
    [SerializeField] private TrailRenderer _tail;

    private PresenterDetector<BlockPresenter> _blockPresenterDetector;

    private bool _isSubscribedBlockPresenterDetector;

    private bool _isInitialized;

    public override void Init()
    {
        base.Init();

        _blockPresenterDetector = new PresenterDetector<BlockPresenter>(_gameObjectColliderDetector);

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
                _blockPresenterDetector.Detected += OnPresenterDetected;
                _blockPresenterDetector.Enable();

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
                _blockPresenterDetector.Disable();
                _blockPresenterDetector.Detected -= OnPresenterDetected;

                _isSubscribedBlockPresenterDetector = false;
            }
        }
    }

    private void OnPresenterDetected(BlockPresenter detectablePresenter)
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