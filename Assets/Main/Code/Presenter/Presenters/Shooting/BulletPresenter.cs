using UnityEngine;

public class BulletPresenter : Presenter
{
    [SerializeField] private CollisionBlockPresenterDetector _blockPresenterDetector;
    [SerializeField] private TrailRenderer _tail;

    protected override void Subscribe()
    {
        base.Subscribe();

        _tail.gameObject.SetActive(true);
        _tail.ResetBounds();
        _blockPresenterDetector.Detected += OnPresenterDetected;
    }

    protected override void Unsubscribe()
    {
        base.Unsubscribe();

        _tail.gameObject.SetActive(false);
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