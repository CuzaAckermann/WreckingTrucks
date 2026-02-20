using UnityEngine;

public interface ITargetAction : ITickable
{
    public void SetTarget(Vector3 target);
}