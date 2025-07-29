using UnityEngine;

public class CheckPoint
{
    public CheckPoint(Vector3 position, Vector3 direction)
    {
        Position = position;
        Direction = direction;
        IsStartOfShooting = false;
        IsFinishOfShooting = false;
    }

    public void StayStarOfShooting()
    {
        IsStartOfShooting = true;
    }

    public void StayFinishOfShooting()
    {
        IsFinishOfShooting = true;
    }

    public Vector3 Position { get; private set; }

    public Vector3 Direction { get; private set; }

    public bool IsStartOfShooting { get; private set; }

    public bool IsFinishOfShooting { get; private set; }
}