public class Bullet : Model
{
    private Block _target;

    public void SetTarget(Block target)
    {
        if (target != null)
        {
            _target = target;
        }
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