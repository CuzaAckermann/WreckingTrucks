using UnityEngine;

public interface IMovable
{
    public void Move(float frameMovement);

    public void SetTargetPosition(Vector3 targetPosition);

    public void FinishMovement();
}