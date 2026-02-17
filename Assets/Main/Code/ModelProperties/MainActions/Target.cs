using UnityEngine;

public class Target<T> where T : Placeable
{
    private readonly T _target;

    public Target(T target)
    {
        Validator.ValidateNotNull(target);

        _target = target;
    }

    public Vector3 Position => _target.Position;
}