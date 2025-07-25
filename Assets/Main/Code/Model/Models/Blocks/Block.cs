using System.Collections.Generic;
using UnityEngine;

public abstract class Block : Model
{
    private readonly List<Vector3> _path;

    public Block()
    {
        _path = new List<Vector3>();
    }

    public bool IsTargetForShooting { get; private set; }

    public override void SetTargetPosition(Vector3 targetPosition)
    {
        base.SetTargetPosition(targetPosition);
    }

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