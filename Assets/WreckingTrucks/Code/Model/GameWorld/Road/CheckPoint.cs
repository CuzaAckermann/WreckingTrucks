using UnityEngine;

public class CheckPoint
{
    public CheckPoint(Vector3 position)
    {
        Position = position;
        IsStartOfShooting = false;
    }

    public void StayStarOfShooting()
    {
        IsStartOfShooting = true;
    }

    public Vector3 Position { get; private set; }

    public bool IsStartOfShooting { get; private set; }
}