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
        PositionManipulator.SetForward((_target.PositionManipulator.Position - PositionManipulator.Position).normalized);
        _target.PositionManipulator.PositionChanged += OnPositionChanged;

        OnPositionChanged();
    }

    public void DestroyBlock(Block block)
    {
        if (block == _target)
        {
            _target.PositionManipulator.PositionChanged -= OnPositionChanged;
            block.Destroy();
            Destroy();
        }
    }

    private void OnPositionChanged()
    {
        Mover.SetTarget(_target.PositionManipulator.Position);
    }

    private void OnDestroyed(IDestroyable block)
    {
        if (block != _target)
        {
            return;
        }

        _target.Destroyed -= OnDestroyed;
        _target.PositionManipulator.PositionChanged -= OnPositionChanged;

        Destroy();
    }
}