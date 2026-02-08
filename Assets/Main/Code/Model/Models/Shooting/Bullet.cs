using System;

public class Bullet : Model
{
    private Block _target;

    public Bullet(PositionManipulator positionManipulator,
                  IMover mover,
                  IRotator rotator)
           : base(positionManipulator,
                  mover,
                  rotator)
    {

    }

    public void SetTarget(Block target)
    {
        _target = target ?? throw new ArgumentNullException(nameof(target));

        // нужна подписка на OnDestroyed
        SetDirectionForward((_target.Position - Position).normalized);
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

    private void OnDestroyed(Model block)
    {
        if (block == _target)
        {
            _target.DestroyedModel -= OnDestroyed;
            _target.PositionChanged -= OnPositionChanged;
            Destroy();
        }
    }
}