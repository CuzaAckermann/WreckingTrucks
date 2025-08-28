using System;

public abstract class Block : Model
{
    public bool IsTargetForShooting { get; private set; }

    public event Action TargetStateChanged;

    public void StayTargetForShooting()
    {
        IsTargetForShooting = true;
        TargetStateChanged?.Invoke();
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