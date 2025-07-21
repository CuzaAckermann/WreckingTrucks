using UnityEngine;

public class BulletPresenter : Presenter
{
    [SerializeField] private CollisionBlockPresenterDetector _blockPresenterDetector;

    protected override void Subscribe()
    {
        base.Subscribe();
        _blockPresenterDetector.PresenterDetected += OnPresenterDetected;
    }

    protected override void Unsubscribe()
    {
        base.Unsubscribe();
        _blockPresenterDetector.PresenterDetected -= OnPresenterDetected;
    }

    private void OnPresenterDetected(BlockPresenter detectablePresenter)
    {
        // —ŒÃÕ»“≈À‹ÕŒ

        if (Model is Bullet bullet)
        {
            if (detectablePresenter.Model is Block block)
            {
                bullet.DestroyBlock(block);
            }
        }
    }
}