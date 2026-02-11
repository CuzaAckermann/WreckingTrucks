using System;
using System.Collections.Generic;
using UnityEngine;

public class Model : IDestroyable, IColorable
{
    private readonly PositionManipulator _positionManipulator;
    private readonly IMover _mover;
    private readonly IRotator _rotator;

    private readonly TraitBearer _traitBearer;

    public Model(PositionManipulator positionManipulator, IMover mover, IRotator rotator)
    {
        _positionManipulator = positionManipulator ?? throw new ArgumentNullException(nameof(positionManipulator));
        _mover = mover ?? throw new ArgumentNullException(nameof(mover));
        _rotator = rotator ?? throw new ArgumentNullException(nameof(rotator));

        _traitBearer = new TraitBearer();
    }

    public event Action<Model> Placed;

    public event Action<IDestroyable> Destroyed;

    public PositionManipulator PositionManipulator => _positionManipulator;

    public IMover Mover => _mover;

    public IRotator Rotator => _rotator;

    public TraitBearer TraitBearer => _traitBearer;

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
        _positionManipulator.SetPosition(position);
        Placed?.Invoke(this);
    }
}