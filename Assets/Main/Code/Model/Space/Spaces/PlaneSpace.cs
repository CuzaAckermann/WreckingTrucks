using System;
using System.Collections.Generic;

public class PlaneSpace : IModelAddedNotifier,
                          IModelDestroyNotifier,
                          IAmountChangedNotifier
{
    private readonly PlaneSlot _planeSlot;
    private readonly Road _road;
    private readonly Mover _planeMover;
    private readonly Rotator _planeRotater;

    public PlaneSpace(PlaneSlot planeSlot,
                      Road road,
                      Mover truckMover,
                      Rotator rotater)
    {
        _planeSlot = planeSlot ?? throw new ArgumentNullException(nameof(planeSlot));
        _road = road ?? throw new ArgumentNullException(nameof(road));
        _planeMover = truckMover ?? throw new ArgumentNullException(nameof(truckMover));
        _planeRotater = rotater ?? throw new ArgumentNullException(nameof(rotater));
    }


    public event Action<Model> ModelAdded;

    public event Action<Model> ModelDestroyRequested;

    public event Action<IModel> InterfaceModelDestroyRequested;

    public event Action<IReadOnlyList<Model>> ModelsDestroyRequested;

    public event Action<IReadOnlyList<IModel>> InterfaceModelsDestroyRequested;

    public event Action<int> AmountChanged;

    public void Clear()
    {
        //InterfaceModelsDestroyRequested?.Invoke(_road.GetTrucks());

        _planeSlot.Clear();
        _road.Clear();
        _planeMover.Clear();
        _planeRotater.Clear();

        _planeSlot.ModelDestroyRequested -= OnModelDestroyRequested;
    }

    public void Prepare()
    {
        _planeSlot.ModelDestroyRequested += OnModelDestroyRequested;

        _planeSlot.Prepare();
    }

    public void Enable()
    {
        //_road.PathFollowerReachedEnd += OnTruckReached;
        _planeSlot.ModelAdded += OnModelAdded;
        _planeSlot.AmountChanged += OnAmountChanged;

        _planeMover.Enable();
        _planeRotater.Enable();
    }

    public void Disable()
    {
        _planeSlot.ModelAdded -= OnModelAdded;
        _planeSlot.AmountChanged -= OnAmountChanged;

        _planeMover.Disable();
        _planeRotater.Disable();

        //_road.PathFollowerReachedEnd -= OnTruckReached;
    }

    public void ReleasePlane(Plane plane)
    {
        _road.AddPlane(plane);
    }

    public bool TryGetPlane(out Plane plane)
    {
        return _planeSlot.TryGetPlane(out plane);
    }

    private void OnModelAdded(Model model)
    {
        ModelAdded?.Invoke(model);
    }

    private void OnAmountChanged(int amount)
    {
        AmountChanged?.Invoke(amount);
    }

    private void OnModelDestroyRequested(Model plane)
    {
        ModelDestroyRequested?.Invoke(plane);
    }
}