using UnityEngine;

public class CartrigeBox : Model
{
    public CartrigeBox(float movespeed, float rotatespeed) : base(movespeed, rotatespeed)
    {

    }

    protected override Vector3 GetAxisOfRotation()
    {
        return Vector3.up;
    }

    protected override Vector3 GetTargetRotation(Vector3 target)
    {
        Vector3 targetDirection = target - Position;
        targetDirection.y = 0;

        return targetDirection;
    }
}