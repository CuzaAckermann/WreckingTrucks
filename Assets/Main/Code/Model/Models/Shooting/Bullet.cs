using System;

public class Bullet : Model
{
    private Block _target;

    public Bullet(Placeable positionManipulator,
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
        Placeable.SetForward((_target.Placeable.Position - Placeable.Position).normalized);
        _target.Placeable.PositionChanged += OnPositionChanged;

        OnPositionChanged();
    }

    public void DestroyBlock(Block block)
    {
        if (block == _target)
        {
            _target.Placeable.PositionChanged -= OnPositionChanged;
            block.Destroy();
            Destroy();
        }
    }

    private void OnPositionChanged()
    {
        Mover.SetTarget(_target.Placeable.Position);
    }

    private void OnDestroyed(IDestroyable block)
    {
        if (block != _target)
        {
            return;
        }

        _target.Destroyed -= OnDestroyed;
        _target.Placeable.PositionChanged -= OnPositionChanged;

        Destroy();
    }
}