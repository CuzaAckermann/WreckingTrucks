using System;
using System.Collections.Generic;
using UnityEngine;

public class PlaneSlot : Model, IModelAddedNotifier,
                                IModelPositionObserver,
                                IModelDestroyNotifier,
                                IAmountChangedNotifier
{
    private readonly PlaneFactory _planeFactory;

    private Plane _plane;
    private int _amountOfUses;

    public PlaneSlot(PlaneFactory planeFactory, Transform position, int amountOfUses)
    {
        if (amountOfUses <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amountOfUses));
        }

        _planeFactory = planeFactory ?? throw new ArgumentNullException(nameof(planeFactory));
        _amountOfUses = amountOfUses;

        SetPosition(position.position);
        SetDirectionForward(position.forward);
    }

    public event Action<int> AmountChanged;

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

    public void Clear()
    {
        ModelDestroyRequested?.Invoke(_plane);
    }

    public void Prepare()
    {
        _plane = _planeFactory.Create();

        _plane.SetPosition(Position + Vector3.right * 10);
        _plane.SetDirectionForward(Forward);

        _plane.SetTargetPosition(Position);

        ModelAdded?.Invoke(_plane);
        ModelPositionChanged?.Invoke(_plane);
        AmountChanged?.Invoke(_amountOfUses);
    }

    public bool TryGetPlane(out Plane plane)
    {
        plane = null;

        if (_amountOfUses > 0)
        {
            plane = _plane;
            _amountOfUses--;
            AmountChanged?.Invoke(_amountOfUses);
            return true;
        }

        return false;
    }
}