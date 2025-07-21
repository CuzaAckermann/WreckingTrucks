using UnityEngine;

public class CheckPoint
{
    public CheckPoint(Vector3 position, Vector3 direction)
    {
        Position = position;
        Direction = direction;
        IsStartOfShooting = false;
    }

    public void StayStarOfShooting()
    {
        IsStartOfShooting = true;
    }

    public Vector3 Position { get; private set; }

    public Vector3 Direction { get; private set; }

    public bool IsStartOfShooting { get; private set; }
}