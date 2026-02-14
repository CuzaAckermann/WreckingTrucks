using System;
using System.Collections.Generic;
using UnityEngine;

public class Model : IDestroyable, IColorable
{
    private readonly Placeable _placeable;
    private readonly IMover _mover;
    private readonly IRotator _rotator;

    private readonly TraitBearer _traitBearer;

    public Model(Placeable positionManipulator, IMover mover, IRotator rotator)
    {
        _placeable = positionManipulator ?? throw new ArgumentNullException(nameof(positionManipulator));
        _mover = mover ?? throw new ArgumentNullException(nameof(mover));
        _rotator = rotator ?? throw new ArgumentNullException(nameof(rotator));

        _traitBearer = new TraitBearer();
    }

    //public event Action<Model> Placed;

    public event Action<IDestroyable> Destroyed;

    public Placeable Placeable => _placeable;

    public IMover Mover => _mover;

    public IRotator Rotator => _rotator;

    public ColorType Color { get; private set; }

    public virtual void Destroy()
    {
        Mover?.Destroy();
        Rotator?.Destroy();

        Destroyed?.Invoke(this);
    }

    public virtual void SetColor(ColorType color)
    {
        Color = color;
    }

    public virtual void SetFirstPosition(Vector3 position)
    {
        _placeable.SetPosition(position);
        //Placed?.Invoke(this);
    }
}