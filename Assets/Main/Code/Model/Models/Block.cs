using System;

public class Block : Model
{
    public Block(float movespeed, float rotatespeed) : base(movespeed, rotatespeed)
    {

    }

    public bool IsTargetForShooting { get; private set; }

    public event Action TargetStateChanged;
    public event Action<Block> BecameTarget;

    public void StayTargetForShooting()
    {
        IsTargetForShooting = true;
        TargetStateChanged?.Invoke();
        BecameTarget?.Invoke(this);
    }

    public void StayFree()
    {
        IsTargetForShooting = false;
        TargetStateChanged?.Invoke();
    }

    public override void Destroy()
    {
        StayFree();
        base.Destroy();
    }
}