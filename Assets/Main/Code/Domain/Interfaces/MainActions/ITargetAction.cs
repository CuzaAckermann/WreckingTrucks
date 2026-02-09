using System;
using UnityEngine;

public interface ITargetAction : IDestroyable
{
    public event Action<ITargetAction> TargetChanged;
    public event Action<ITargetAction> TargetReached;

    public void SetTarget(Vector3 target);

    public void DoStep(float stepSize);
}