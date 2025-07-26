public abstract class Block : Model
{
    public bool IsTargetForShooting { get; private set; }

    public void StayTargetForShooting()
    {
        IsTargetForShooting = true;
    }

    public override void Destroy()
    {
        IsTargetForShooting = false;
        base.Destroy();
    }
}