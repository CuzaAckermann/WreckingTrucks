using UnityEngine;

public abstract class BlockPresenter : Presenter
{
    [SerializeField] private bool _isTarget;

    protected override void Subscribe()
    {
        if (Model is Block block)
        {
            block.TargetStateChanged += OnTargetStateChanged;
        }

        base.Subscribe();
    }

    protected override void Unsubscribe()
    {
        if (Model is Block block)
        {
            block.TargetStateChanged -= OnTargetStateChanged;
        }

        base.Unsubscribe();
    }

    private void OnTargetStateChanged()
    {
        if (Model is Block block)
        {
            _isTarget = block.IsTargetForShooting;
        }
    }
}