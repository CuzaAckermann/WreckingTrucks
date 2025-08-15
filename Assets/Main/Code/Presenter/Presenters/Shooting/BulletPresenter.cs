using UnityEngine;

public class BulletPresenter : Presenter
{
    [SerializeField] private CollisionBlockPresenterDetector _blockPresenterDetector;

    protected override void Subscribe()
    {
        base.Subscribe();

        _blockPresenterDetector.Detected += OnPresenterDetected;
    }

    protected override void Unsubscribe()
    {
        base.Unsubscribe();

        _blockPresenterDetector.Detected -= OnPresenterDetected;
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