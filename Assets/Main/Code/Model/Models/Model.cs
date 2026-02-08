using System;
using UnityEngine;

public class Model : IDestroyable
{
    private readonly PositionManipulator _positionManipulator;
    private readonly IMover _mover;
    private readonly IRotator _rotator;
    
    public Model(PositionManipulator positionManipulator, IMover mover, IRotator rotator)
    {
        _positionManipulator = positionManipulator ?? throw new ArgumentNullException(nameof(positionManipulator));
        _mover = mover ?? throw new ArgumentNullException(nameof(mover));
        _rotator = rotator ?? throw new ArgumentNullException(nameof(rotator));

        _positionManipulator.PositionChanged += OnPositionChanged;
        _positionManipulator.RotationChanged += OnRotationChanged;

        _mover.TargetPositionChanged += OnTargetPositionChanged;
        _mover.TargetPositionReached += OnTargetPositionReached;

        _rotator.TargetRotationChanged += OnTargetRotationChanged;
        _rotator.TargetRotationReached += OnTargetRotationReached;
    }

    public event Action PositionChanged;
    public event Action RotationChanged;

    public event Action<Model> FirstPositionDefined;

    public event Action<Model> TargetPositionChanged;
    public event Action<Model> TargetPositionReached;

    public event Action<Model> TargetRotationChanged;
    public event Action<Model> TargetRotationReached;

    public event Action<IDestroyable> DestroyedIDestroyable;
    public event Action<Model> DestroyedModel;

    public Vector3 Position => _positionManipulator.Position;

    public Vector3 Forward => _positionManipulator.Forward;

    public Vector3 DirectionToTarget => _mover.DirectionToTarget;

    public Vector3 TargetPosition => _mover.TargetPosition;

    public Vector3 TargetForRotation => _rotator.TargetForRotation;

    public ColorType ColorType { get; private set; }

    public virtual void Destroy()
    {
        _positionManipulator.PositionChanged -= OnPositionChanged;
        _positionManipulator.RotationChanged -= OnRotationChanged;

        _mover.TargetPositionChanged -= OnTargetPositionChanged;
        _mover.TargetPositionReached -= OnTargetPositionReached;

        _rotator.TargetRotationChanged -= OnTargetRotationChanged;
        _rotator.TargetRotationReached -= OnTargetRotationReached;

        DestroyedModel?.Invoke(this);
        DestroyedIDestroyable?.Invoke(this);
    }

    public virtual void SetColor(ColorType color)
    {
        ColorType = color;
    }

    public virtual void SetDirectionForward(Vector3 forward)
    {
        _rotator.SetForward(forward);
    }

    public virtual void SetTargetRotation(Vector3 target)
    {
        _rotator.SetTargetRotation(target);
    }

    public virtual void SetFirstPosition(Vector3 position)
    {
        _mover.SetPosition(position);
        FirstPositionDefined?.Invoke(this);
    }

    public virtual void SetPosition(Vector3 position)
    {
        _mover.SetPosition(position);
    }

    public virtual void SetTargetPosition(Vector3 targetPosition)
    {
        _mover.SetTargetPosition(targetPosition);
    }

    public virtual void Move(float frameMovement)
    {
        _mover.Move(frameMovement);
    }

    public virtual void Rotate(float frameRotation)
    {
        _rotator.Rotate(frameRotation);
    }

    protected virtual void FinishMovement()
    {
        _mover.FinishMovement();
    }

    private void OnPositionChanged()
    {
        PositionChanged?.Invoke();
    }

    private void OnRotationChanged()
    {
        RotationChanged?.Invoke();
    }

    private void OnTargetPositionChanged()
    {
        TargetPositionChanged?.Invoke(this);
    }

    private void OnTargetPositionReached()
    {
        TargetPositionReached?.Invoke(this);
    }

    private void OnTargetRotationChanged()
    {
        TargetRotationChanged?.Invoke(this);
    }

    private void OnTargetRotationReached()
    {
        TargetRotationReached?.Invoke(this);
    }
}