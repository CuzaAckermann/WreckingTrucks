using System;
using System.Collections.Generic;
using UnityEngine;

public class PlaneSlot : Model, IModelAddedNotifier,
                                IModelPositionObserver,
                                IModelDestroyNotifier
{
    private readonly PlaneFactory _planeFactory;

    public PlaneSlot(PlaneFactory planeFactory, Transform position)
    {
        _planeFactory = planeFactory ?? throw new ArgumentNullException(nameof(planeFactory));
        SetPosition(position.position);
        SetDirectionForward(position.forward);
    }

    public event Action<Model> ModelAdded;

    public event Action<Model> ModelPositionChanged;

    public event Action<Model> PositionReached;
    public event Action<IModel> InterfacePositionChanged;
    public event Action<List<Model>> PositionsChanged;
    public event Action<List<IModel>> InterfacePositionsChanged;

    public event Action<Model> ModelDestroyRequested;
    public event Action<IModel> InterfaceModelDestroyRequested;
    public event Action<IReadOnlyList<Model>> ModelsDestroyRequested;
    public event Action<IReadOnlyList<IModel>> InterfaceModelsDestroyRequested;

    public Plane Plane { get; private set; }

    public void Clear()
    {
        ModelDestroyRequested?.Invoke(Plane);
    }

    public void Prepare()
    {
        Plane = _planeFactory.Create();

        Plane.SetPosition(Position + Vector3.right * 10);
        Plane.SetDirectionForward(Forward);

        Plane.SetTargetPosition(Position);

        ModelAdded?.Invoke(Plane);
        ModelPositionChanged?.Invoke(Plane);
    }
}