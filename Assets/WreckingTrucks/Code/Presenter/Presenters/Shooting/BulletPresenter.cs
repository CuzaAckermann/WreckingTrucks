using UnityEngine;

public class BulletPresenter : Presenter
{
    [SerializeField] private CollisionBlockPresenterDetector _collisionBlockPresenterDetector;

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
        if (Model is Bullet bullet && detectablePresenter.Model is Block block)
        {
            bullet.DestroyBlock(block);
        }
    }
}