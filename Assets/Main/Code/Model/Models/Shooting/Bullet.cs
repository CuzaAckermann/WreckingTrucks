using System;

public class Bullet : Model
{
    private Block _target;

    public void SetTarget(Block target)
    {
        _target = target ?? throw new ArgumentNullException(nameof(target));

        // нужна подписка на OnDestroyed
        _target.PositionChanged += OnPositionChanged;

        OnPositionChanged();
    }

    public void DestroyBlock(Block block)
    {
        if (block == _target)
        {
            _target.PositionChanged -= OnPositionChanged;
            block.Destroy();
            Destroy();
        }
    }

    private void OnPositionChanged()
    {
        SetTargetPosition(_target.Position);
    }
}