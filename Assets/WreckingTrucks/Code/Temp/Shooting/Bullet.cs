using System;

public class Bullet : Model
{
    private Block _target;

    public void SetTarget(Block target)
    {
        _target = target ?? throw new ArgumentNullException(nameof(target));
        SetTargetPosition(_target.Position);
    }

    public void DestroyBlock(Block block)
    {
        if (block == _target)
        {
            block.Destroy();
            Destroy();
        }
    }
}