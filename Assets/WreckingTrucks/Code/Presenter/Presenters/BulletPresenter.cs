using UnityEngine;

public class BulletPresenter : Presenter
{
    [SerializeField] private CollisionBlockPresenterDetector _collisionBlockPresenterDetector;

    private BlockPresenter _detectablePresenter;

    public void SetDetectablePresenter(BlockPresenter detectablePresenter)
    {
        _detectablePresenter = detectablePresenter;
    }

    protected override void Subscribe()
    {
        base.Subscribe();
        _collisionBlockPresenterDetector.PresenterDetected += OnPresenterDetected;
    }

    protected override void Unsubscribe()
    {
        base.Unsubscribe();
        _collisionBlockPresenterDetector.PresenterDetected -= OnPresenterDetected;
    }

    private void OnPresenterDetected(BlockPresenter detectablePresenter)
    {
        if (detectablePresenter == _detectablePresenter)
        {
            if (Model is Bullet bullet && detectablePresenter.Model is Block block)
            {
                bullet.DestroyBlock(block);
            }
        }
    }
}