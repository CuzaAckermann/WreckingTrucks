using UnityEngine;

public class JellyVertex
{
    public int ID;
    public Vector3 Position;
    public Vector3 Velocity;
    public Vector3 Force;

    public JellyVertex(int id, Vector3 position)
    {
        ID = id;
        Position = position;
    }

    public bool TryShake(Vector3 target, float mass, float stiffness, float damping)
    {
        Force = (target - Position) * stiffness;
        Velocity = (Velocity + Force / mass) * damping;
        Position += Velocity;

        if ((Velocity + Force + Force / mass).sqrMagnitude < 0.001f)
        {
            Position = target;
            return false;
        }

        return true;
    }

    public void Settle(Vector3 target)
    {
        Position = target;
    }
}